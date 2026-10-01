using ResolveApi.Models;

namespace ResolveApi.Dtos.Requests
{
    public class CreateTicketDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TicketPriority Priority { get; set; }
    }
}