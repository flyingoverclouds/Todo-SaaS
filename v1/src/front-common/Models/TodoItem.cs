using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace front_common.Models
{
    public class TodoItem
    {
        [JsonProperty(PropertyName="id")]
        public Guid Id { get; set; } = Guid.NewGuid();
        [JsonPropertyName("partitionKey")]
        public string Tenant { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public string Title { get; set; }
        public string Content { get; set; }
        public bool Done { get; set; } = false;

    }
}
