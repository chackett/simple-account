using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SimpleAccount.DTO.Response
{
    public class Transaction
    {
        [JsonPropertyName("transaction_id")]
        public string Id { get; set; }

        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("amount")]
        public float Amount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("transaction_type")]
        public string Type { get; set; }

        [JsonPropertyName("transaction_category")]
        public string Category { get; set; }

        [JsonPropertyName("transaction_classifications")]
        public string[] Classification { get; set; }

        [JsonPropertyName("merchant_name")]
        public string MerchantName { get; set; }

        [JsonPropertyName("running_balance")]
        public string RunningBalance { get; set; }

        [JsonPropertyName("meta")]
        public Dictionary<string, object> Meta { get; set; }
    }
}