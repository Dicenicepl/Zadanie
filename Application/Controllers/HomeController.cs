using Application.Models;
using Application.Models.DTO;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Application.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtTokenService _jwtTokenService;

        public HomeController(AppDbContext context, JwtTokenService jwtTokenService)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginDto loginUser)
        {
            var errors = new Dictionary<string, string[]>();

            var user = _context.users.FirstOrDefault(x => x.Email == loginUser.Email);

            if (user == null)
            {
                errors["User"] = new[] { "User does not exist." };
            }
            else if (!BCrypt.Net.BCrypt.Verify(loginUser.Password, user.HashedPassword))
            {
                errors["Password"] = new[] { "Password is incorrect." };
            }
            if (errors.Any())
            {
                return BadRequest(ApiResponse.Fail("Validation failed", errors));
            }

            var token = _jwtTokenService.GenerateToken(user);

            return Ok(new { token });
        }

        [HttpPost("register")]

        public async Task<IActionResult> Register([FromBody] RegisterDto registerUser)
        {
            var errors = new Dictionary<string, string[]>();

            if (registerUser.Password != registerUser.ConfirmPassword)
                errors["Password"] = new[] { "Passwords do not match." };

            if (await _context.users.AnyAsync(u => u.Email == registerUser.Email))
                errors["Email"] = new[] { "Email is already registered." };

            if (errors.Any())
                return BadRequest(ApiResponse.Fail("Validation failed", errors));

            var user = new User
            {
                Email = registerUser.Email,
                HashedPassword = BCrypt.Net.BCrypt.HashPassword(registerUser.Password),
                FirstName = registerUser.FirstName,
                LastName = registerUser.LastName
            };
            _context.users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(ApiResponse.Ok(message: "User registered successfully."));
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var email = User.FindFirstValue(ClaimTypes.Name);
            if (email == null) return Unauthorized();

            var user = await _context.users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return NotFound();

            return Ok(new
            {
                user.Email,
                user.FirstName,
                user.LastName,
                user.CreatedDate
            });
        }
    }
}
