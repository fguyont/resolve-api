using System.ComponentModel.DataAnnotations.Schema;

namespace ResolveApi.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TicketStatus Status { get; set; } = TicketStatus.OPEN;
        public TicketPriority Priority { get; set; } = TicketPriority.MEDIUM;
        public string? AiAnalysis { get; set; }
        public int CreatedById { get; set; }
        [ForeignKey("CreatedById")]
        public User? Creator { get; set; }
        public int? AssignedAgentId { get; set; }
        [ForeignKey("AssignedAgentId")]
        public User? AssignedAgent { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}