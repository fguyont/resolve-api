using Microsoft.AspNetCore.Mvc;
using ResolveApi.Dtos.Requests;
using ResolveApi.IServices;

namespace ResolveApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            var success = await _userService.RegisterAsync(registerRequest);

            if (!success)
            {
                return BadRequest(new { message = "Register failed." });
            }

            return Ok(new { message = "Register succeeded." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var token = await _userService.LoginAsync(loginRequest);
            if (token == null)
            {
                return Unauthorized(new { message = "Login failed." });
            }

            return Ok(new { token = token, message = "Login succeeded." });
        }
    }
}