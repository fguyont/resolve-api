using ResolveApi.Models;

namespace ResolveApi.Services
{
    public interface ITicketService
    {
        Task<IEnumerable<Ticket>> GetTicketsAsync(TicketStatus? status);
        Task<Ticket> CreateTicketAsync(Ticket ticket);
        Task<Ticket?> UpdateStatusAsync(int id, TicketStatus status);
    }
}
