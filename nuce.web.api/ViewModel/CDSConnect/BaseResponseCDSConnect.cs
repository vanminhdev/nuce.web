using System.Text.Json.Serialization;

namespace nuce.web.api.ViewModel.CDSConnect
{
    public class BaseResponseCDSConnect
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
        //public object Data { get; set; }
    }
}
