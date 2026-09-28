using ResolveApi.Dtos.Requests;

namespace ResolveApi.IServices
{
    public interface IUserService
    {
        Task<bool> RegisterAsync(RegisterRequest registerRequest);
        Task<string?> LoginAsync(LoginRequest loginRequest);
    }
}