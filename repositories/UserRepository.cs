using Microsoft.EntityFrameworkCore;
using ResolveApi.Data;
using ResolveApi.IRepositories;
using ResolveApi.Models;

namespace ResolveApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAgentsAsync()
        {
            return await _context.Users
                .Where(u => u.Role == Role.AGENT)
                .ToListAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
