using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntis
{
    public interface IWebUntis
    {
        /// <summary>
        /// The schools name being sent along requests.
        /// E.g. "pamina sz"
        /// </summary>
        public string School { get; set; }
        /// <summary>
        /// The identifier of the server this school is located on.
        /// E.g. Your school runs under https://dummy.webuntis.com/ then "dummy" is all you need to fill in here.
        /// </summary>
        public string ServerIdent { get; set; }
        /// <summary>
        /// The username to login with, usually your email-address.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// The password to be logged in with.
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// A custom client-identifier.
        /// </summary>
        public string ClientIdentifier { get; set; }

        /// <summary>
        /// Get session information about the current session.
        /// </summary>
        /// <returns>SessionToken, UserId and UserType</returns>
        public WebUntisSession Session { get; }
        /// <summary>
        /// Returns only the session-token as string.
        /// </summary>
        /// <returns></returns>
        public string SessionToken { get => Session.SessionToken; }

        /// <summary>
        /// Retrieve current package-version.
        /// </summary>
        /// <returns></returns>
        public Version GetVersion();
        /// <summary>
        /// Authenticate with the server, create a session and return it.
        /// The sessiontoken is stored internally and used for subsequent requests.
        /// </summary>
        /// <returns>True if login was successful.</returns>
        public Task<bool> Login();
        /// <summary>
        /// Terminates a session with the WebUntis-Server.
        /// The sessiontoken is cleared and KeepAlive disabled.
        /// </summary>
        /// <returns></returns>
        public Task<bool> Logout();
        /// <summary>
        /// Enables automatic keep-alive of the current session.
        /// </summary>
        /// <returns>True if auto-keepalive successfully enabled.</returns>
        public Task<bool> EnableKeepSessionAlive();
        /// <summary>
        /// Disables automatic keep-alive of the current session.
        /// </summary>
        /// <returns>True if auto-keepalive was successfully disabled.</returns>
        public Task<bool> DisableKeepSessionAlive();
        /// <summary>
        /// Refresh the current session.
        /// </summary>
        /// <returns>True if successful.</returns>
        public Task<bool> RefreshSession();

        public Task<List<WebUntisTimeTableEntry>> GetTimeTableToday(WebUntisItemType itemType, int itemId);
        public Task<List<WebUntisTimeTableEntry>> GetTimeTableCurrentWeek(WebUntisItemType itemType, int itemId);
        public Task<List<WebUntisTimeTableEntry>> GetTimeTableNextWeek(WebUntisItemType itemType, int itemId);
        public Task<List<WebUntisTimeTableEntry>> GetTimeTable(WebUntisItemType itemType, int itemId, DateTime start, DateTime end);

        /*
        public Task<WebUntisExams> GetExams(int studentId) => GetExams(studentId, DateTime.Now, DateTime.Now);
        public Task<WebUntisExams> GetExams(int studentId, DateTime start, DateTime end);
        */
    }
}
