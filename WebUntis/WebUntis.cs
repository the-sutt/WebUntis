using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace WebUntis
{
    public enum WebUntisClassType {
        Lesson,
        Exam,
        Cancelled,
        Irregular
    }

    public class WebUntis : IWebUntis
    {
        #region Public Properties
        public string School { get; set; }
        public string ServerIdent {
            get => _serverIdent;
            set
            {
                _serverIdent = value;
                UpdateBaseAddress();
            }
        }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ClientIdentifier { get; set; }

        public WebUntisSession Session { get; set; }
        #endregion

        #region Private Properties
        HttpClient _client;
        HttpClientHandler _handler;

        CancellationTokenSource _keepAliveTokenSource;
        CancellationToken _keepAliveToken;

        private string _serverIdent;
        #endregion

        #region Constructor
        public WebUntis()
        {
            _handler = new() { UseCookies = false };
            _client = new(_handler);

            _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.79 Safari/537.36");
            _client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            _client.DefaultRequestHeaders.Add("Pragma", "no-cache");
            _client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
        }
        #endregion

        #region GenericFunctions
        public Version GetVersion() => Assembly.GetExecutingAssembly().GetName().Version;
        #endregion

        #region Session
        public async Task<bool> DisableKeepSessionAlive()
        {
            try
            {
                _keepAliveTokenSource.Cancel();
                return await Task.FromResult(true);
            }
            catch
            {
                return await Task.FromResult(false);
            }
        }

        public async Task<bool> EnableKeepSessionAlive()
        {
            try
            {
                _keepAliveTokenSource?.Cancel();
                _keepAliveTokenSource = new CancellationTokenSource();
                _keepAliveToken = _keepAliveTokenSource.Token;

                var _  = Task.Run(async () =>
                {
                    while (!_keepAliveToken.IsCancellationRequested)
                    {
                        var res = await RefreshSession();
                        if (!res) {
                            // refresh session failed - try re-login until success
                            while (!_keepAliveToken.IsCancellationRequested)
                            {
                                var loginRes = await Login();
                                if (loginRes) break;
                                await Task.Delay(10_000);
                            }
                        }
                        await Task.Delay(180_000);
                    }
                });
                return await Task.FromResult(true);
            }
            catch
            {
                return await Task.FromResult(false);
            }
        }

        public async Task<bool> RefreshSession()
        {
            var req = new WebUntisRequestBase()
            {
                Method = WebUntisRequestType.getLatestImportTime
            };
            req.Id = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

            try
            {
                var res = await SendJsonAndDeserializeResult<WebUntisRequestBase, WebUntisRequestBase>(req);
                if ((res.Result as JsonElement?).GetValueOrDefault().GetInt64() > 0)
                {
                    return true;
                }
            }
            catch (Exception ex) {
                Console.WriteLine("FAILED: "+ex.Message);
//                Console.WriteLine(ex);
            }
            Console.WriteLine("FAILED w/o error");
            return false;
        }
        #endregion

        #region Timetable
        public async Task<List<WebUntisTimeTableEntry>> GetTimeTable(WebUntisItemType itemType, int itemId, DateTime start, DateTime end)
        {
            var req = new WebUntisRequestTimeTable()
            {
                Id = DateTimeOffset.Now.ToUnixTimeSeconds().ToString(),
                Method = WebUntisRequestType.getTimetable
            };
            req.Params = new()
            {
                Options = new()
                {
                    Id = DateTimeOffset.Now.ToUnixTimeSeconds().ToString(),
                    Element = new()
                    {
                        Id = itemId,
                        Type = itemType
                    },
                    UntisStartDate = start.ToUntisDate(),
                    UntisEndDate = end.ToUntisDate()
                }
            };

            try
            {
                var res = await SendJsonAndDeserializeResult<WebUntisRequestTimeTable, WebUntisRequestTimeTableResult>(req);
                return res.Entries.OrderBy(c => c.EntryDate).ThenBy(c => c.EntryStartTime).ToList();
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public Task<List<WebUntisTimeTableEntry>> GetTimeTableToday(WebUntisItemType itemType, int itemId) => GetTimeTable(itemType, itemId, DateTime.Now, DateTime.Now);

        public Task<List<WebUntisTimeTableEntry>> GetTimeTableCurrentWeek(WebUntisItemType itemType, int itemId) => GetTimeTable(itemType, itemId, DateTime.Now.StartOfWeek(DayOfWeek.Monday), DateTime.Now.StartOfWeek(DayOfWeek.Monday).AddDays(7));

        public Task<List<WebUntisTimeTableEntry>> GetTimeTableNextWeek(WebUntisItemType itemType, int itemId) => GetTimeTable(itemType, itemId, DateTime.Now.StartOfWeek(DayOfWeek.Monday).AddDays(7), DateTime.Now.StartOfWeek(DayOfWeek.Monday).AddDays(14));
        #endregion

        #region Login Logout
        public async Task<bool> Login()
        {
            var req = new WebUntisRequestLogin()
            {
                Method = WebUntisRequestType.authenticate
            };
            req.Id = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            req.Params = new()
            {
                Client = ClientIdentifier,
                Username = Username,
                Password = Password
            };

            try
            {
                var sessionInfo = await SendJsonAndDeserializeResult<WebUntisRequestLogin, WebUntisRequestLoginResult>(req);
                Session = sessionInfo.Session;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Logout()
        {
            var req = new WebUntisRequestBase()
            {
                Method = WebUntisRequestType.logout
            };
            req.Id = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

            try
            {
                var _ = await SendJsonAndDeserializeResult<WebUntisRequestBase, WebUntisRequestBase>(req);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region Private Methods
        private void UpdateBaseAddress() => _client.BaseAddress = new Uri($"https://{_serverIdent}.webuntis.com/WebUntis/jsonrpc.do");

        private async Task<Tres> SendJsonAndDeserializeResult<T, Tres>(T request)
        {
            var message = new HttpRequestMessage(HttpMethod.Post, _client.BaseAddress + "?school=" + School);
            message.Headers.Add("Cookie", "JSESSIONID="+Session?.SessionToken);
            var str = JsonSerializer.Serialize<T>(request);
            message.Content = new StringContent(str, Encoding.UTF8, "application/json");
            var response = await _client.SendAsync(message);

            response.EnsureSuccessStatusCode();

            var resJson = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Tres>(resJson);

            var error = (request as WebUntisRequestBase).Error;
            if (error != default)
                throw new Exception($"WebUntis: error #{error.Code}: {error.Message}");
            return result;
        }
        #endregion
    }
}
