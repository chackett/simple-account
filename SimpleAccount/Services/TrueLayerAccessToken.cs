using System.Text.Json.Serialization;
using Microsoft.Extensions.Hosting.Internal;

namespace SimpleAccount.Services
{
    public class TrueLayerAccessToken
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
        
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }
        
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
    }
}