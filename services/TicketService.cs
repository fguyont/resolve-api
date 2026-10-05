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
        private readonly ICurrentUserService _currentUserService;

        public TicketService(
            ITicketRepository ticketRepository,
            IGeminiService geminiService,
            ICurrentUserService currentUserService)
        {
            _ticketRepository = ticketRepository;
            _geminiService = geminiService;
            _currentUserService = currentUserService;
        }

        private int? GetCurrentUserIdInt()
        {
            var rawId = _currentUserService.GetUserId();
            if (rawId == null) return null;
            
            if (int.TryParse(rawId.ToString(), out int id))
            {
                return id;
            }
            return null;
        }

        public async Task<IEnumerable<TicketDto>> GetTicketsAsync(bool includeArchived = false)
        {
            var userId = GetCurrentUserIdInt();
            bool isAgent = _currentUserService.IsAgent();

            var tickets = await _ticketRepository.GetAllAsync(userId, isAgent, includeArchived);

            return tickets.Select(MapToDto);
        }

        public async Task<TicketDto?> GetTicketByIdAsync(int id)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null) return null;

            var userId = GetCurrentUserIdInt();
            bool isAgent = _currentUserService.IsAgent();

            if (!isAgent && ticket.CreatedById != userId)
            {
                throw new UnauthorizedAccessException("Vous n'êtes pas autorisé à consulter ce ticket.");
            }

            return MapToDto(ticket);
        }

        public async Task<TicketDto> CreateTicketAsync(CreateTicketDto dto)
        {
            var userId = GetCurrentUserIdInt() ?? throw new UnauthorizedAccessException("Utilisateur non identifié.");

            var aiAnalysis = await _geminiService.AnalyzeTicketAsync(dto.Title, dto.Description);

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
                CreatedById = userId,
                AssignedAgentId = null,
                CreatedAt = DateTime.UtcNow
            };

            await _ticketRepository.AddAsync(ticket);

            var createdTicket = await _ticketRepository.GetByIdAsync(ticket.Id);
            return MapToDto(createdTicket ?? ticket);
        }

        public async Task<TicketDto?> UpdateTicketContentAsync(int id, UpdateTicketDto dto)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null) return null;

            var userId = GetCurrentUserIdInt();
            bool isAgent = _currentUserService.IsAgent();

            if (!isAgent && ticket.CreatedById != userId)
            {
                throw new UnauthorizedAccessException("Vous n'êtes pas autorisé à modifier ce ticket.");
            }

            ticket.Title = dto.Title;
            ticket.Description = dto.Description;

            if (isAgent)
            {
                ticket.Priority = dto.Priority;
            }

            ticket.UpdatedAt = DateTime.UtcNow;

            await _ticketRepository.UpdateAsync(ticket);
            
            var updatedTicket = await _ticketRepository.GetByIdAsync(id);
            return MapToDto(updatedTicket ?? ticket);
        }

        public async Task<TicketDto?> UpdateTicketStatusAsync(int id, TicketStatus newStatus)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null) return null;

            if (!_currentUserService.IsAgent())
            {
                throw new UnauthorizedAccessException("Les clients ne peuvent pas modifier le statut des tickets.");
            }

            if (ticket.AssignedAgentId == null)
            {
                ticket.AssignedAgentId = GetCurrentUserIdInt();
            }

            ticket.Status = newStatus;
            ticket.UpdatedAt = DateTime.UtcNow;

            await _ticketRepository.UpdateAsync(ticket);
            
            var updatedTicket = await _ticketRepository.GetByIdAsync(id);
            return MapToDto(updatedTicket ?? ticket);
        }

        public async Task<TicketDto?> ArchiveTicketAsync(int id)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null) return null;

            var userId = GetCurrentUserIdInt();
            bool isAgent = _currentUserService.IsAgent();

            if (!isAgent && ticket.CreatedById != userId)
            {
                throw new UnauthorizedAccessException("Action non autorisée.");
            }

            ticket.Status = TicketStatus.ARCHIVED;
            ticket.UpdatedAt = DateTime.UtcNow;

            await _ticketRepository.UpdateAsync(ticket);
            
            var updatedTicket = await _ticketRepository.GetByIdAsync(id);
            return MapToDto(updatedTicket ?? ticket);
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
                CreatedById = ticket.CreatedById,
                CreatedByName = ticket.User?.Name ?? "Unknown",
                AssignedAgentId = ticket.AssignedAgentId,
                CreatedAt = ticket.CreatedAt,
                UpdatedAt = ticket.UpdatedAt
            };
        }
    }
}