namespace ResolveApi.Models
{
    public enum TicketStatus { OPEN, IN_PROGRESS, RESOLVED, CLOSED }
    public enum TicketPriority { LOW, MEDIUM, HIGH, CRITICAL }

    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TicketStatus Status { get; set; } = TicketStatus.OPEN;
        public TicketPriority Priority { get; set; } = TicketPriority.MEDIUM;
        public string? AiAnalysis { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}