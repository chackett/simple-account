using Microsoft.Extensions.Hosting.Internal;
using System.Text.Json.Serialization;
using SimpleAccount.Services;

namespace SimpleAccount.DTO.Response
{
    public class AccountList
    {
        [JsonPropertyName("results")]
        public Account[] Accounts { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}