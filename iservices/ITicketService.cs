using ResolveApi.Dtos.Requests;
using ResolveApi.Dtos.Responses;

namespace ResolveApi.IServices
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketDto>> GetTicketsAsync(bool includeArchived = false);
        Task<TicketDto?> GetTicketByIdAsync(int id);
        Task<TicketDto> CreateTicketAsync(CreateTicketDto dto);
        Task<TicketDto?> UpdateTicketAsync(int id, UpdateTicketDto dto);
        Task<TicketDto?> ArchiveTicketAsync(int id);
    }
}
