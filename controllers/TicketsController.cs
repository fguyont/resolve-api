using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResolveApi.Data;
using ResolveApi.Models;

namespace ResolveApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TicketsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        async public Task<ActionResult<IEnumerable<Ticket>>> GetTickets([FromQuery] TicketStatus? status)
        {
            var query = _context.Tickets.AsQueryable();
            if (status.HasValue)
            {
                query = query.Where(t => t.Status == status.Value);
            }
            return await query.ToListAsync();
        }

        [HttpPost]
        async public Task<ActionResult<Ticket>> CreateTicket(Ticket ticket)
        {
            ticket.Id = 0;
            if (ticket.Description.Contains("urgent", StringComparison.OrdinalIgnoreCase) || 
                ticket.Description.Contains("down", StringComparison.OrdinalIgnoreCase))
            {
                ticket.Priority = TicketPriority.HIGH;
            }

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTickets), new { id = ticket.Id }, ticket);
        }

        [HttpPatch("{id}/status")]
        async public Task<ActionResult<Ticket>> UpdateStatus(int id, [FromQuery] TicketStatus status)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return NotFound();

            ticket.Status = status;
            ticket.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(ticket);
        }
    }
}