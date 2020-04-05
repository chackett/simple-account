using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SimpleAccount.DTO.Response
{
    public class TransactionList
    {
        [JsonPropertyName("results")]
        public List<Transaction> Transactions { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}