using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResolveApi.Dtos.Requests;
using ResolveApi.Dtos.Responses;
using ResolveApi.IServices;

namespace ResolveApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketDto>>> GetTickets([FromQuery] bool includeArchived = false)
        {
            var tickets = await _ticketService.GetTicketsAsync(includeArchived);
            return Ok(tickets);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TicketDto>> GetTicketById(int id)
        {
            try
            {
                var ticket = await _ticketService.GetTicketByIdAsync(id);
                if (ticket == null)
                {
                    return NotFound();
                }
                return Ok(ticket);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid(); // Renvoie un 403 Forbidden si l'utilisateur n'a pas le droit d'y accéder
            }
        }

        [HttpPost]
        public async Task<ActionResult<TicketDto>> CreateTicket([FromBody] CreateTicketDto dto)
        {
            var createdTicket = await _ticketService.CreateTicketAsync(dto);
            return CreatedAtAction(nameof(GetTickets), new { id = createdTicket.Id }, createdTicket);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TicketDto>> UpdateTicketContent(int id, [FromBody] UpdateTicketDto dto)
        {
            try
            {
                var updatedTicket = await _ticketService.UpdateTicketContentAsync(id, dto);
                return Ok(updatedTicket);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Ticket not found." });
            }
        }

        [HttpPatch("{id}/status")]
        public async Task<ActionResult<TicketDto>> UpdateTicketStatus(int id, [FromBody] UpdateStatusDto dto)
        {
            try
            {
                var updatedTicket = await _ticketService.UpdateTicketStatusAsync(id, dto.Status);
                return Ok(updatedTicket);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Key not found." });
            }
        }

        [HttpPatch("{id}/archive")]
        public async Task<ActionResult<TicketDto>> ArchiveTicket(int id)
        {
            try
            {
                var archivedTicket = await _ticketService.ArchiveTicketAsync(id);
                return Ok(archivedTicket);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Ticket not found." });
            }
        }
    }
}