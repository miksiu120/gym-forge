using System.Security.Claims;
using WorkPlanner.Application.Abstractions;

namespace WorkPlanner.Infrastructure.Identity;

public sealed class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public int GetRequiredUserId()
    {
        var principal = httpContextAccessor.HttpContext?.User;
        if (principal?.Identity?.IsAuthenticated != true)
        {
            throw new UnauthorizedAccessException("User is not authorized.");
        }

        var value = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(value, out var userId))
        {
            throw new UnauthorizedAccessException("The user id is missing from the token.");
        }

        return userId;
    }
}
