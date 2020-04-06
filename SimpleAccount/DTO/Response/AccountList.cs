using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SimpleAccount.DTO.Response
{
    public class AccountList
    {
        [JsonPropertyName("results")] public List<Account> Accounts { get; set; }

        [JsonPropertyName("status")] public string Status { get; set; }
    }
}