using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebUntis
{
    public class WebUntisRequestTimeTable : WebUntisRequestBase
    {
        [JsonPropertyName("params")]
        public WebUntisRequestTimeTableParameters Params { get; set; }
    }

    public class WebUntisRequestTimeTableParameters
    {
        [JsonPropertyName("options")]
        public WebUntisRequestTimeTableOptions Options { get; set; }
    }

    public class WebUntisRequestTimeTableResult
    {
        [JsonPropertyName("result")]
        public List<WebUntisTimeTableEntry> Entries { get; set; }
    }

    public class WebUntisRequestTimeTableOptions
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("startDate")]
        public long UntisStartDate { get; set; }
        [JsonPropertyName("endDate")]
        public long UntisEndDate { get; set; }
        [JsonPropertyName("element")]
        public WebUntisRequestElement Element { get; set; }
        [JsonPropertyName("showLsText")]
        public bool ShowLsText { get => true; }
        [JsonPropertyName("showStudentgroup")]
        public bool ShowStudentGroup { get => true; }
        [JsonPropertyName("showLsNumber")]
        public bool ShowLsNumber { get => true; }
        [JsonPropertyName("showSubstText")]
        public bool ShowSubstText { get => true; }
        [JsonPropertyName("showInfo")]
        public bool ShowInfo { get => true; }
        [JsonPropertyName("showBooking")]
        public bool ShowBooking { get => true; }

        [JsonPropertyName("klasseFields")]
        public string[] FieldsKlasse { get => new[] { "id", "name", "longname", "externalkey" }; }
        [JsonPropertyName("roomFields")]
        public string[] FieldsRoom { get => new[] { "id", "name", "longname", "externalkey" }; }
        [JsonPropertyName("subjectFields")]
        public string[] FieldsSubject { get => new[] { "id", "name", "longname", "externalkey" }; }
        [JsonPropertyName("teacherFields")]
        public string[] FieldsTeacher { get => new[] { "id", "name", "longname", "externalkey" }; }
    }
}
