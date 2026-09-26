using Microsoft.AspNetCore.Mvc;
using ResolveApi.Models;
using ResolveApi.Services;

namespace ResolveApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets([FromQuery] TicketStatus? status)
        {
            var tickets = await _ticketService.GetTicketsAsync(status);
            return Ok(tickets);
        }

        [HttpPost]
        public async Task<ActionResult<Ticket>> CreateTicket(Ticket ticket)
        {
            var createdTicket = await _ticketService.CreateTicketAsync(ticket);
            return CreatedAtAction(nameof(GetTickets), new { id = createdTicket.Id }, createdTicket);
        }

        [HttpPatch("{id}/status")]
        public async Task<ActionResult<Ticket>> UpdateStatus(int id, [FromQuery] TicketStatus status)
        {
            var updatedTicket = await _ticketService.UpdateStatusAsync(id, status);
            if (updatedTicket == null) return NotFound();

            return Ok(updatedTicket);
        }
    }
}