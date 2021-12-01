using System;
using System.Text.Json.Serialization;

namespace Domain
{
    public class Category
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("categoryName")]
        public string CategoryName { get; set; } = "";
        [JsonPropertyName("categorySort")]
        public int CategorySort { get; set; } = 0;
        [JsonPropertyName("syncDt")]
        public DateTime SyncDt { get; set; } = DateTime.MinValue;
    }
}