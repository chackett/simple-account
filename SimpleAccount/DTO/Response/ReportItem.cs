using System.Text.Json.Serialization;

namespace SimpleAccount.DTO.Response
{
    public class ReportItem
    {
        [JsonPropertyName("category")] public string Category { get; set; }

        [JsonPropertyName("value")] public float Value { get; set; }
    }
}