using ResolveApi.Models;

namespace ResolveApi.IRepositories
{
    public interface ITicketRepository
    {
        Task<IEnumerable<Ticket>> GetAllAsync(bool includeArchived = false);
        Task<Ticket?> GetByIdAsync(int id);
        Task AddAsync(Ticket ticket);
        Task UpdateAsync(Ticket ticket);
    }
}