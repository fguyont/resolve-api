using Microsoft.EntityFrameworkCore;
using ResolveApi.Models;
using ResolveApi.Data;
using ResolveApi.IRepositories;

namespace ResolveApi.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly AppDbContext _context;

        public TicketRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ticket>> GetAllAsync(bool includeArchived = false)
        {
            IQueryable<Ticket> query = _context.Tickets;

            if (!includeArchived)
            {
                query = query.Where(t => t.Status != TicketStatus.ARCHIVED);
            }
            query = query.OrderByDescending(t => t.UpdatedAt ?? t.CreatedAt);
            return await query.ToListAsync();
        }

        public async Task<Ticket?> GetByIdAsync(int id)
        {
            return await _context.Tickets.FindAsync(id);
        }

        public async Task AddAsync(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
        }
    }
}