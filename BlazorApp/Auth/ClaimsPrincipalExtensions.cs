using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorApp.Auth;

public static class ClaimsPrincipalExtensions
{
    public static int? GetUserId(this AuthenticationState state)
    {
        ClaimsPrincipal claimsPrincipal = state.User;
        if (claimsPrincipal.Identity is null || !claimsPrincipal.Identity.IsAuthenticated)
        {
            return -1; // User is not authenticated
        }
        string? userName = claimsPrincipal.Identity.Name;
        IEnumerable<Claim> claims = claimsPrincipal.Claims;
        string userIdAsString = claims.FirstOrDefault(
            c => c.Type == ClaimTypes.NameIdentifier)!.Value ?? string.Empty;
        return int.Parse(userIdAsString);
    }
}
