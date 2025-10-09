using Application.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services
{
    public class JwtTokenService
    {
        private readonly IConfiguration _config;

        public JwtTokenService(IConfiguration config)
        {
            _config = config;
        }
        public string GenerateToken(User user)
        {
            var jwt = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]));

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email)
            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
               issuer: jwt["Issuer"],
               audience: jwt["Audience"],
               claims: claims,
               expires: DateTime.UtcNow.AddHours(double.Parse(jwt["ExpireHours"] ?? "2")),
               signingCredentials: creds
           );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
