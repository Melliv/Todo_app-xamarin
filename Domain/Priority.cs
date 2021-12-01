using System;
using System.Text.Json.Serialization;

namespace Domain
{
    public class Priority
    {
        //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("id")]
        public string Id { get; set; } = "00000000-0000-0000-0000-000000000000";
        [JsonPropertyName("priorityName")]
        public string PriorityName { get; set; } = "";

        [JsonPropertyName("prioritySort")] 
        public int PrioritySort { get; set; } = 0;
        [JsonPropertyName("syncDt")]
        public DateTime SyncDt { get; set; } = DateTime.MinValue;
    }
}