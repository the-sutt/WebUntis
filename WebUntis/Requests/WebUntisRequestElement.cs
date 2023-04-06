using System.Text.Json.Serialization;

namespace WebUntis
{
    public class WebUntisRequestElement
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("type")]
        public WebUntisItemType Type { get; set; }
    }
}
