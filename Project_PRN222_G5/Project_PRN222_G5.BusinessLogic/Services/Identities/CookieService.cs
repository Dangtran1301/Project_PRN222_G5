using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Identities;
using Project_PRN222_G5.DataAccess.DTOs.Users.Responses;
using System.Security.Claims;

namespace Project_PRN222_G5.BusinessLogic.Services.Identities;

public class CookieService(IHttpContextAccessor httpContextAccessor) : ICookieService
{
    private const string AccessTokenCookieName = "Project_PRN222_G5.Web.AccessToken";

    public async Task SetAuthCookiesAsync(UserResponse user, string accessToken, string refreshToken)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("RefreshToken", refreshToken),
            new Claim("uid", user.Id.ToString())
        };

        var claimsIdentity = new ClaimsIdentity(claims, "Project_PRN222_G5.Web.Cookies");
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext is null) throw new InvalidOperationException("HttpContext is not available.");

        await httpContext.SignInAsync(
            "Project_PRN222_G5.Web.Cookies",
            claimsPrincipal,
            new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            });

        httpContext.Response.Cookies.Append(AccessTokenCookieName, accessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddHours(1)
        });
    }

    public async Task RemoveAuthCookiesAsync()
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null) throw new InvalidOperationException("HttpContext is not available.");

        await httpContext.SignOutAsync("Project_PRN222_G5.Web.Cookies");
        httpContext.Response.Cookies.Delete(AccessTokenCookieName);
    }

    public string? GetRefreshToken()
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext is null) return null;

        return httpContext.User.FindFirst("RefreshToken")?.Value;
    }
}