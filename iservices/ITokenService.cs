using ResolveApi.Models;

namespace ResolveApi.IServices
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}