using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace front_common.Models
{
    public class TodoItem
    {
        
        public Guid id { get; set; } = Guid.NewGuid();
                
        public string tenant { get; set; }

        public DateTime timestamp { get; set; } = DateTime.UtcNow;

        
        public string title { get; set; }
        

        public string content { get; set; }

        public bool done { get; set; } = false;

    }
}
