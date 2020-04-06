using System.Text.Json.Serialization;

namespace SimpleAccount.DTO.Response
{
    public class RunningBalance
    {
        [JsonPropertyName("amount")] public float Amount { get; set; }

        [JsonPropertyName("currency")] public string Meta { get; set; }
    }
}