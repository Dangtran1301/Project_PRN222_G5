using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace Project_PRN222_G5.Web.Utilities;

internal static class ClaimsUtil
{
    public static string GetUsername(ClaimsPrincipal principal)
    {
        if (principal == null!) return "Anonymous";

        return principal.FindFirstValue(JwtRegisteredClaimNames.Sub)
               ?? "Anonymous";
    }
}