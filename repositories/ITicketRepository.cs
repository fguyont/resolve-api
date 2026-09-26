using ResolveApi.Models;

namespace ResolveApi.Repositories
{
    public interface ITicketRepository
    {
        Task<IEnumerable<Ticket>> GetTicketsAsync(TicketStatus? status);
        Task<Ticket?> GetByIdAsync(int id);
        Task<Ticket> AddAsync(Ticket ticket);
        Task UpdateAsync(Ticket ticket);
    }
}