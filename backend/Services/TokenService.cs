using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WorkPlanner.Entities;

namespace WorkPlanner.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
        string GenerateRefreshToken(User user);
    }

    public class TokenService : ITokenService
    {

        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
      
        }

        public string GenerateToken(User user)
        {
            var secretKey = _configuration["Authentication:JwtKey"];
            var issuer = _configuration["Authentication:JwtIssuer"];
            var audience = _configuration["Authentication:JwtIssuer"]; 
            var tokenExpireHours = int.Parse(_configuration["Authentication:JwtExpireAccount"]);

            var claims = new List<Claim>()
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role),
        new Claim(ClaimTypes.Name, user.Nickname)
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddHours(tokenExpireHours); 

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience, 
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public string GenerateRefreshToken(User user)
        {
            var secretKey = _configuration["Authentication:JwtKey"];
            var issuer = _configuration["Authentication:JwtIssuer"];
            var refreshTokenExpireDays = int.Parse(_configuration["Authentication:JwtRefreshTokenAccount"]);

            var claims = new List<Claim>
            {
                new Claim("userId", user.Id.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: issuer,
                expires: DateTime.Now.AddDays(refreshTokenExpireDays),
                signingCredentials: creds,
                claims: claims
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
