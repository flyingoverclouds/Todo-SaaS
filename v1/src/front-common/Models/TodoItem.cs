namespace front_common.Models
{
    public class TodoItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Tenant { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public string Title { get; set; }
        public string Content { get; set; }
        public bool Done { get; set; } = false;

    }
}
