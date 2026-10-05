using ResolveApi.Dtos.Requests;
using ResolveApi.Dtos.Responses;
using ResolveApi.Models;

namespace ResolveApi.IServices
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketDto>> GetTicketsAsync(bool includeArchived = false);
        Task<TicketDto?> GetTicketByIdAsync(int id);
        Task<TicketDto> CreateTicketAsync(CreateTicketDto dto);
        Task<TicketDto?> UpdateTicketContentAsync(int id, UpdateTicketDto dto);
        Task<TicketDto?> UpdateTicketStatusAsync(int id, TicketStatus newStatus);
        Task<TicketDto?> ArchiveTicketAsync(int id);
    }
}
