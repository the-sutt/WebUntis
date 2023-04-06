using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebUntis
{
    public class WebUntisRequestLastImportTime : WebUntisRequestBase
    {
        [JsonPropertyName("params")]
        public WebUntisRequestLastImportTimeParameters Params { get; set; }
    }

    public class WebUntisRequestLastImportTimeParameters
    {
        [JsonPropertyName("user")]
        public string Username { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
        [JsonPropertyName("client")]
        public string Client { get; set; }
    }

    public class WebUntisRequestLastImportTimeResult
    {
        [JsonPropertyName("result")]
        public WebUntisSession Session { get; set; }
    }
}
