using ResolveApi.Models;

namespace ResolveApi.Dtos.Requests
{
    public class RegisterRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ClearPassword { get; set; } = string.Empty;
        public Role Role { get; set; } = Role.CLIENT;
    }
}