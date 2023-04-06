using System;
using System.Text.Json.Serialization;

namespace WebUntis
{
    public enum WebUntisRequestType
    {
        undefined,
        authenticate,
        logout,
        getTimetable,
        getLatestImportTime
    }

    public class WebUntisRequestBase
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("method"), JsonConverter(typeof(JsonStringEnumConverter))]
        public WebUntisRequestType Method { get; set; }
        [JsonPropertyName("jsonrpc")]
        public string JsonRpc { get => "2.0"; }
        [JsonPropertyName("result")]
        public object Result { get; set; }
        [JsonPropertyName("error")]
        public WebUntisResponseError Error { get; set; }
    }

    public class WebUntisResponseError
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("code")]
        public int Code { get; set; }
    }
}
