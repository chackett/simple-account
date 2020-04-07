using System.Text.Json.Serialization;

namespace SimpleAccount.DTO.Response
{
    public class AuthorisationCallbackResponse
    {
        [JsonPropertyName("provider")] public string Message { get; set; }
    }
}