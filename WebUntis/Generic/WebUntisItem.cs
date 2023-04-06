using System.Text.Json.Serialization;

namespace WebUntis
{
    public enum WebUntisItemType
    {
        Class = 1,
        Teacher = 2,
        Subject = 3,
        Room = 4,
        Student = 5,
        Parents = 15
    }

    public class WebUntisItem
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("longname")]
        public string LongName { get; set; }
    }
}
