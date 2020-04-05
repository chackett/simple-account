using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Hosting.Internal;

namespace SimpleAccount.DTO.Response
{
    public class CategorySummaryReport
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("error")]
        public string Error { get; set; }

        [JsonPropertyName("result")]
        public List<ReportItem> Result { get; }
        
        public CategorySummaryReport()
        {
            Result = new List<ReportItem>();
        }
    }
}