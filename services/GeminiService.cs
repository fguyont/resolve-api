using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using ResolveApi.Models.Gemini;

namespace ResolveApi.Services;

public class GeminiService : IGeminiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public GeminiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["Gemini:ApiKey"] ?? string.Empty;
    }

    public async Task<string> AnalyzeTicketAsync(string ticketTitle, string ticketDescription)
    {
        // Prompt preparing
        var prompt = $"Analyze the following support ticket and provide category suggestions and priority in English:\n\nTitle: {ticketTitle}\nDescription: {ticketDescription}";
        var requestBody = new GeminiRequest
        {
            Contents = new List<GeminiContent>
            {
                new GeminiContent
                {
                    Parts = new List<GeminiPart>
                    {
                        new GeminiPart { Text = prompt }
                    }
                }
            }
        };

        // Google model choosing
        var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-3.6-flash:generateContent?key={_apiKey}";

        // Request sending
        var response = await _httpClient.PostAsJsonAsync(url, requestBody);
        
        if (!response.IsSuccessStatusCode)
        {
            var errorDetails = await response.Content.ReadAsStringAsync();
            return $"Erreur Gemini ({response.StatusCode}): {errorDetails}";
        }

        // Response reading
        var result = await response.Content.ReadFromJsonAsync<GeminiResponse>();

        // Response extracting
        var generatedText = result?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text;

        return generatedText ?? "No response.";
    }
}