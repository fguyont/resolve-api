namespace ResolveApi.Services;

public interface IGeminiService
{
    Task<string> AnalyzeTicketAsync(string ticketTitle, string ticketDescription);
}