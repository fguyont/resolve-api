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
                throw new UnauthorizedAccessException("You are not allowed.");
            }

            return MapToDto(ticket);
        }

        public async Task<TicketDto> CreateTicketAsync(CreateTicketDto dto)
        {
            var userId = GetCurrentUserIdInt() ?? throw new UnauthorizedAccessException("User not identified.");

            // Default priority set
            TicketPriority priority = TicketPriority.MEDIUM;

            string aiAnalysis = await AnalyzeTicket(dto.Title, dto.Description);
            if (IsPriorityHigh(dto.Description, aiAnalysis) == true)
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

        public async Task<TicketDto?> UpdateTicketAsync(int id, UpdateTicketDto dto)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null) return null;

            var userId = GetCurrentUserIdInt();
            bool isAgent = _currentUserService.IsAgent();

            if (!isAgent && ticket.CreatedById != userId)
            {
                throw new UnauthorizedAccessException("You are not allowed");
            }

            ticket.Title = dto.Title;
            ticket.Description = dto.Description;

            // New analysis after update
            string aiAnalysis = await AnalyzeTicket(dto.Title, dto.Description);
            
            if (IsPriorityHigh(dto.Description, aiAnalysis) == true)
            {
                ticket.Priority = TicketPriority.HIGH;
            }
            ticket.AiAnalysis = aiAnalysis;

            if (isAgent)
            {
                ticket.Priority = dto.Priority;
                ticket.Status = ticket.Status == TicketStatus.OPEN ? TicketStatus.IN_PROGRESS : dto.Status;
                ticket.AssignedAgentId = dto.AssignedAgentId ?? userId;
            }

            ticket.UpdatedAt = DateTime.UtcNow;

            await _ticketRepository.UpdateAsync(ticket);

            var updatedTicket = await _ticketRepository.GetByIdAsync(id);
            return MapToDto(updatedTicket ?? ticket);
        }

        public async Task<TicketDto?> ArchiveTicketAsync(int id)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null) return null;

            if (!_currentUserService.IsAgent())
            {
                throw new UnauthorizedAccessException("Only agents can delete tickets.");
            }

            if (ticket.AssignedAgentId == null)
            {
                ticket.AssignedAgentId = GetCurrentUserIdInt();
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
                CreatedByName = ticket.Creator?.Name ?? "Unknown",
                AssignedAgentId = ticket.AssignedAgentId,
                AssignedAgentName = ticket.AssignedAgent?.Name,
                CreatedAt = ticket.CreatedAt,
                UpdatedAt = ticket.UpdatedAt
            };
        }

        private async Task<string> AnalyzeTicket(string title, string description)
        {
            return await _geminiService.AnalyzeTicketAsync(title, description);
        }

        private static bool IsPriorityHigh (string description, string aiAnalysis)
        {
            if (aiAnalysis.Contains("HIGH", StringComparison.OrdinalIgnoreCase) ||
                description.Contains("urgent", StringComparison.OrdinalIgnoreCase) ||
                description.Contains("down", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }
    }
}