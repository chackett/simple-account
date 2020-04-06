using System.Text.Json.Serialization;

namespace SimpleAccount.DTO.Response
{
    public class Account
    {
        [JsonPropertyName("update_timestamp")] public string UpdateTimestamp { get; set; }

        [JsonPropertyName("account_id")] public string AccountId { get; set; }

        [JsonPropertyName("account_type")] public string AccountType { get; set; }

        [JsonPropertyName("display_name")] public string DisplayName { get; set; }

        [JsonPropertyName("currency")] public string Currency { get; set; }

        [JsonPropertyName("account_number")] public AccountNumber AccountNumber { get; set; }

        [JsonPropertyName("provider")] public Provider Provider { get; set; }
    }
}