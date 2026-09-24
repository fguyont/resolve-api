using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResolveApi.Data;
using ResolveApi.Models;
using ResolveApi.Services;

namespace ResolveApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IGeminiService _geminiService;

        public TicketsController(AppDbContext context, IGeminiService geminiService)
        {
            _context = context;
            _geminiService = geminiService;
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

            // We ask GeminiService
            var aiAnalysis = await _geminiService.AnalyzeTicketAsync(ticket.Title, ticket.Description);

            // We give additional instructions
            if (aiAnalysis.Contains("HIGH", StringComparison.OrdinalIgnoreCase) || 
                ticket.Description.Contains("urgent", StringComparison.OrdinalIgnoreCase) || 
                ticket.Description.Contains("down", StringComparison.OrdinalIgnoreCase))
            {
                ticket.Priority = TicketPriority.HIGH;
            }

            ticket.AiAnalysis = aiAnalysis;
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            // Response
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