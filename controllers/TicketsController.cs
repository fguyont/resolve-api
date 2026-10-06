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
                return Forbid();
            }
        }

        [HttpPost]
        public async Task<ActionResult<TicketDto>> CreateTicket([FromBody] CreateTicketDto dto)
        {
            var createdTicket = await _ticketService.CreateTicketAsync(dto);
            return CreatedAtAction(nameof(GetTickets), new { id = createdTicket.Id }, createdTicket);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TicketDto>> UpdateTicket(int id, [FromBody] UpdateTicketDto dto)
        {
            try
            {
                var updatedTicket = await _ticketService.UpdateTicketAsync(id, dto);
                return Ok(updatedTicket);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Ticket not found." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
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