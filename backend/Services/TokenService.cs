using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WorkPlanner.Configs;
using WorkPlanner.Entities;

namespace WorkPlanner.Services;

public interface ITokenService
{
    string GenerateToken(User user);
    string GenerateRefreshToken(User user);
}

public sealed class TokenService(IOptions<AuthenticationSettings> options) : ITokenService
{
    private readonly AuthenticationSettings _settings = options.Value;

    public string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role),
            new(ClaimTypes.Name, user.Nickname)
        };

        return CreateToken(claims, DateTime.UtcNow.AddHours(_settings.JwtExpireAccount));
    }

    public string GenerateRefreshToken(User user)
    {
        var claims = new List<Claim>
        {
            new("userId", user.Id.ToString())
        };

        return CreateToken(
            claims,
            DateTime.UtcNow.AddDays(_settings.JwtRefreshTokenAccount));
    }

    private string CreateToken(IEnumerable<Claim> claims, DateTime expires)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.JwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _settings.JwtIssuer,
            audience: _settings.JwtIssuer,
            claims: claims,
            expires: expires,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
