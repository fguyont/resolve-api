namespace ResolveApi.Services
{
    public interface ICurrentUserService
    {
        string? GetUserId();
        string? GetUserRole();
        bool IsAgent();
    }
}