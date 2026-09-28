namespace ResolveApi.IServices
{
    public interface IGeminiService
    {
        Task<string> AnalyzeTicketAsync(string ticketTitle, string ticketDescription);
    }
}