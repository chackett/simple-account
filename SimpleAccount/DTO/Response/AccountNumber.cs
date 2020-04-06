using System.Text.Json.Serialization;

namespace SimpleAccount.DTO.Response
{
    public class AccountNumber
    {
        [JsonPropertyName("iban")] public string Iban { get; set; }

        [JsonPropertyName("swift_bic")] public string SwiftBic { get; set; }

        [JsonPropertyName("number")] public string Number { get; set; }

        [JsonPropertyName("sort_code")] public string SortCode { get; set; }
    }
}