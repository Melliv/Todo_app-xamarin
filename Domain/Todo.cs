using System;
using System.Text.Json.Serialization;

namespace Domain
{
    public class Todo
    {
        //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("id")]
        public string Id { get; set; } = "00000000-0000-0000-0000-000000000000";
        [JsonPropertyName("taskName")]
        public string TaskName { get; set; } = "";
        [JsonPropertyName("taskSort")] 
        public int TaskSort { get; set; } = 0;
        [JsonPropertyName("createdDt")]
        public DateTime CreatedDt { get; set; } = DateTime.MinValue;
        [JsonPropertyName("dueDt")]
        public DateTime DueDt { get; set; } = DateTime.MinValue;
        [JsonPropertyName("isCompleted")]
        public bool IsCompleted { get; set; } = false;

        [JsonPropertyName("isArchived")] 
        public bool IsArchived { get; set; } = false;
        [JsonPropertyName("todoCategoryId")]
        public string TodoCategoryId { get; set; }
        [JsonPropertyName("todoPriorityId")]
        public string TodoPriorityId { get; set; }
        [JsonPropertyName("syncDt")]
        public DateTime SyncDt { get; set; } = DateTime.MinValue;
    }
}