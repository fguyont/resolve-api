using ResolveApi.Dtos.Requests;
using ResolveApi.Dtos.Responses;

namespace ResolveApi.IServices
{
    public interface IUserService
    {
        Task<IEnumerable<AgentDto>> GetAgentsAsync();
        Task<bool> RegisterAsync(RegisterRequest registerRequest);
        Task<string?> LoginAsync(LoginRequest loginRequest);
    }
}