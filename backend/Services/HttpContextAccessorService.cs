using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace PartyGame.Services
{
    public interface IHttpContextAccessorService
    {
        string GetTokenFromHeader();
        int? GetUserIdFromToken();
    }

    public class HttpContextAccessorService : IHttpContextAccessorService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextAccessorService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public string GetTokenFromHeader()
        {
            var authorizationHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                throw new KeyNotFoundException("Authorization header or token is missing.");
            }

            string token = authorizationHeader.Substring("Bearer ".Length).Trim();

            if (string.IsNullOrEmpty(token))
            {
                throw new KeyNotFoundException("Token was not found in the Authorization header.");
            }

            return token;
        }

        public int? GetUserIdFromToken()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || !user.Identity?.IsAuthenticated == true)
            {
                throw new UnauthorizedAccessException("User is not authorized.");
            }

            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new InvalidOperationException("User not found in token.");
            }

            if (int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            else
            {
                throw new InvalidOperationException("ID użytkownika w tokenie nie jest prawidłową liczbą całkowitą.");
            }
        }


    }
}

