using Microsoft.EntityFrameworkCore;
using ResolveApi.Models;

namespace ResolveApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Ticket> Tickets => Set<Ticket>();
    }
}