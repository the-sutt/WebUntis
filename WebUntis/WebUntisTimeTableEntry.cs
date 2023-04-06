using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace WebUntis
{
    public class WebUntisTimeTableEntry
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("date")]
        public long UntisDate { get; set; }
        [JsonPropertyName("startTime")]
        public long UntisStartTime { get; set; }
        [JsonPropertyName("endTime")]
        public long UntisEndTime { get; set; }

        [JsonPropertyName("kl")] // Klasse(n)
        public List<WebUntisItem> Classes { get; set; }
        [JsonPropertyName("te")] // Teacher(s)
        public List<WebUntisItem> Teachers { get; set; }
        [JsonPropertyName("su")] // Subject(s)
        public List<WebUntisItem> Subjects { get; set; }
        [JsonPropertyName("ro")] // Rooms(s)
        public List<WebUntisItem> Rooms { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("activityType")]
        public string ActivityType { get; set; }

        [JsonPropertyName("sg")] // Student Group
        public string Sg { get; set; }

        [JsonPropertyName("lstext")] // Lesson Text
        public string LsText { get; set; }

        // convenience properties below
        public bool IsCancelled { get => Code == "cancelled"; }
        public bool IsIrregular { get => Code == "irregular"; }

        public DateTime EntryDate {
            get => DateTimeExtensions.FromUntisDate(UntisDate);
            set { EntryDate = value; UntisDate = DateTimeExtensions.ToUntisDate(value); }
        }
        public TimeSpan EntryStartTime {
            get => DateTimeExtensions.FromUntisTime(UntisStartTime);
            set { EntryStartTime = value; UntisStartTime = DateTimeExtensions.ToUntisTime(value); }
        }
        public TimeSpan EntryEndTime {
            get => DateTimeExtensions.FromUntisTime(UntisEndTime);
            set { EntryEndTime = value; UntisEndTime = DateTimeExtensions.ToUntisTime(value); }
        }

        public WebUntisClassType ClassType { get {
                if (IsCancelled) return WebUntisClassType.Cancelled;
                if (Sg == null)
                    if (Code == "irregular") return WebUntisClassType.Irregular;
                if (Sg.ToUpper().Contains("EXAM")) return WebUntisClassType.Exam;
                return WebUntisClassType.Lesson;
            } }

        public string Subject { get {
                return ClassType == WebUntisClassType.Exam ? LsText : Subjects.FirstOrDefault()?.LongName ?? "<unknown>";
            } }
    }
}
