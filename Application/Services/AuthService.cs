using Application.Models;
using Application.Models.DTO;
using Application.Repositories;
using Application.Repositories.Interfaces;
using Application.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtTokenService _jwtTokenService;

        public AuthService(IUserRepository userRepository, JwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<UserDto> GetUserAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null) return null;

            return new UserDto
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CreatedDate = user.CreatedDate
            };
        }

        public async Task<ApiResponse> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetUserByEmailAsync(dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.HashedPassword))
                return ApiResponse.Fail("Invalid credentials.");

            var token = _jwtTokenService.GenerateToken(user);
            return ApiResponse.Ok(new { token }, "Login successful.");
        }

        public async Task<ApiResponse> RegisterAsync(RegisterDto dto)
        {
            var errors = new Dictionary<string, string[]>();

            if (dto.Password != dto.ConfirmPassword)
                errors["Password"] = new[] { "Passwords do not match." };

            if (await _userRepository.EmailAlreadyExistsAsync(dto.Email))
                errors["Email"] = new[] { "Email already registered." };

            if (errors.Any())
                return ApiResponse.Fail("Validation failed", errors);

            var user = new User
            {
                Email = dto.Email,
                HashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };

            await _userRepository.AddAsync(user);
            return ApiResponse.Ok(message: "User registered successfully.");
        }
    }
}
