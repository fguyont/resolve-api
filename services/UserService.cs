using Microsoft.AspNetCore.Identity;
using ResolveApi.Models;
using ResolveApi.Dtos.Requests;
using ResolveApi.IRepositories;
using ResolveApi.IServices;

namespace ResolveApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly PasswordHasher<User> _passwordHasher = new();

        public UserService(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<bool> RegisterAsync(RegisterRequest registerRequest)
        {
            var existingUser = await _userRepository.GetByEmailAsync(registerRequest.Email);
            if (existingUser != null)
            {
                return false;
            }


            var user = new User
            {
                Email = registerRequest.Email,
                Name = registerRequest.Name,
                Role = registerRequest.Role
            };

            user.Password = _passwordHasher.HashPassword(user, registerRequest.ClearPassword);

            await _userRepository.AddAsync(user);
            return true;
        }

        public async Task<string?> LoginAsync(LoginRequest loginRequest)
        {
            var user = await _userRepository.GetByEmailAsync(loginRequest.Email);
            if (user == null)
            {
                return null;
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, loginRequest.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return _tokenService.GenerateToken(user);
        }
    }
}