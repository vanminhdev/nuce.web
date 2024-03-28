using nuce.web.api.ViewModel.CDSConnect;
using System.Text.Json.Serialization;

namespace nuce.web.api.ViewModel.MotCuaConnect
{
    public class ResponseMotCuaUsernameDto
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }
    }
}
