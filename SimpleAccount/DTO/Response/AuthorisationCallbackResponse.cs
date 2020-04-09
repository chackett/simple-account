using System.Text.Json.Serialization;

namespace SimpleAccount.DTO.Response
{
    public class AuthorisationCallbackResponse
    {
        [JsonPropertyName("message")] public string Message { get; set; }
    }
}