using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebUntis
{
    public class WebUntisSession
    {
        [JsonPropertyName("sessionId")]
        public string SessionToken { get; set; }
        [JsonPropertyName("personId")]
        public int UserId { get; set; }
        [JsonPropertyName("personType")]
        public WebUntisItemType UserType { get; set; }
    }
}
