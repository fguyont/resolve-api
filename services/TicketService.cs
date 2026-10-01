using ResolveApi.Dtos.Requests;
using ResolveApi.Dtos.Responses;
using ResolveApi.IRepositories;
using ResolveApi.IServices;
using ResolveApi.Models;

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

        public async Task<IEnumerable<TicketDto>> GetTicketsAsync(bool includeArchived = false)
        {
            var tickets = await _ticketRepository.GetAllAsync(includeArchived);
            return tickets.Select(MapToDto);
        }

        public async Task<TicketDto> CreateTicketAsync(CreateTicketDto dto)
        {
            // Gemini analysis
            var aiAnalysis = await _geminiService.AnalyzeTicketAsync(dto.Title, dto.Description);

            // Classification rules depending on ticket description and AI analysis
            var priority = TicketPriority.MEDIUM;
            if (aiAnalysis.Contains("HIGH", StringComparison.OrdinalIgnoreCase) || 
                dto.Description.Contains("urgent", StringComparison.OrdinalIgnoreCase) || 
                dto.Description.Contains("down", StringComparison.OrdinalIgnoreCase))
            {
                priority = TicketPriority.HIGH;
            }

            var ticket = new Ticket
            {
                Title = dto.Title,
                Description = dto.Description,
                Priority = priority,
                Status = TicketStatus.OPEN,
                AiAnalysis = aiAnalysis,
                CreatedAt = DateTime.UtcNow
            };

            await _ticketRepository.AddAsync(ticket);
            return MapToDto(ticket);
        }

        public async Task<TicketDto?> UpdateTicketContentAsync(int id, UpdateTicketDto dto)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null) return null;

            ticket.Title = dto.Title;
            ticket.Description = dto.Description;
            ticket.Priority = dto.Priority;
            ticket.UpdatedAt = DateTime.UtcNow;

            await _ticketRepository.UpdateAsync(ticket);
            return MapToDto(ticket);
        }

        public async Task<TicketDto?> UpdateTicketStatusAsync(int id, TicketStatus newStatus)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null) return null;

            ticket.Status = newStatus;
            ticket.UpdatedAt = DateTime.UtcNow;

            await _ticketRepository.UpdateAsync(ticket);
            return MapToDto(ticket);
        }

        public async Task<TicketDto?> ArchiveTicketAsync(int id)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null) return null;

            ticket.Status = TicketStatus.ARCHIVED;
            ticket.UpdatedAt = DateTime.UtcNow;

            await _ticketRepository.UpdateAsync(ticket);
            return MapToDto(ticket);
        }

        private static TicketDto MapToDto(Ticket ticket)
        {
            return new TicketDto
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Description = ticket.Description,
                Status = ticket.Status,
                Priority = ticket.Priority,
                AiAnalysis = ticket.AiAnalysis,
                CreatedAt = ticket.CreatedAt,
                UpdatedAt = ticket.UpdatedAt
            };
        }
    }
}