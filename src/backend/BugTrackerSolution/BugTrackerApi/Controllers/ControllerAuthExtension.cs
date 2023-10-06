using System.Security.Claims;

namespace BugTrackerApi.Controllers;

public static class ControllerAuthExtension
{
    public static string GetName(this ClaimsPrincipal claims)
    {
        return claims.Identity?.Name ?? throw new ApplicationException("Something is really wrong with Auth");
    }
}