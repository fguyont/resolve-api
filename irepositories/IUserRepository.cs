using ResolveApi.Models;

namespace ResolveApi.IRepositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAgentsAsync();
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
    }
}