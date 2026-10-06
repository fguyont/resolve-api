using Microsoft.AspNetCore.Mvc;
using ResolveApi.Dtos.Requests;
using ResolveApi.Dtos.Responses;
using ResolveApi.IServices;
using ResolveApi.Services;

namespace ResolveApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICurrentUserService _currentUserService;

        public AuthController(IUserService userService, ICurrentUserService currentUserService)
        {
            _userService = userService;
            _currentUserService = currentUserService;
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
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest loginRequest)
        {
            var token = await _userService.LoginAsync(loginRequest);
            if (token == null)
            {
                return Unauthorized(new { message = "Login failed." });
            }

            var response = new AuthResponse
            {
                Token = token,
                Username = loginRequest.Email,
                ExpiresAt = DateTime.UtcNow.AddMinutes(30)
            };

            return Ok(response);
        }

        [HttpGet("me")]
        public IActionResult GetCurrentUserInfo()
        {
            var userId = _currentUserService.GetUserId();
            var isAgent = _currentUserService.IsAgent();

            return Ok(new
            {
                userId = userId,
                isAgent = isAgent
            });
        }
    }
}