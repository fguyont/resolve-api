using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResolveApi.Dtos.Responses;
using ResolveApi.IServices;

namespace ResolveApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("agents")]
        public async Task<ActionResult<IEnumerable<AgentDto>>> GetAgents()
        {
            var agents = await _userService.GetAgentsAsync();
            return Ok(agents);
        }
    }
}