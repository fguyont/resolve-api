using ResolveApi.Models;

namespace ResolveApi.Dtos.Requests
{
    public class UpdateTicketDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TicketPriority Priority { get; set; }
    }
}