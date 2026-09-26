using ResolveApi.Models;
using ResolveApi.Repositories;

namespace ResolveApi.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IGeminiService _geminiService;

        public TicketService(ITicketRepository ticketRepository, IGeminiService geminiService)
        {
            _ticketRepository = ticketRepository;
            _geminiService = geminiService;
        }

        public async Task<IEnumerable<Ticket>> GetTicketsAsync(TicketStatus? status)
        {
            return await _ticketRepository.GetTicketsAsync(status);
        }

        public async Task<Ticket> CreateTicketAsync(Ticket ticket)
        {
            ticket.Id = 0;

            // Gemini analysis
            var aiAnalysis = await _geminiService.AnalyzeTicketAsync(ticket.Title, ticket.Description);

            // Classification rules depending of ticket description and AI analysis
            if (aiAnalysis.Contains("HIGH", StringComparison.OrdinalIgnoreCase) || 
                ticket.Description.Contains("urgent", StringComparison.OrdinalIgnoreCase) || 
                ticket.Description.Contains("down", StringComparison.OrdinalIgnoreCase))
            {
                ticket.Priority = TicketPriority.HIGH;
            }

            ticket.AiAnalysis = aiAnalysis;
            return await _ticketRepository.AddAsync(ticket);
        }

        public async Task<Ticket?> UpdateStatusAsync(int id, TicketStatus status)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null) return null;

            ticket.Status = status;
            ticket.UpdatedAt = DateTime.UtcNow;

            await _ticketRepository.UpdateAsync(ticket);
            return ticket;
        }
    }
}