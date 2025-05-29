using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Identities;
using System.Security.Claims;

namespace Project_PRN222_G5.BusinessLogic.Services.Identities;

public class CookieService(IHttpContextAccessor httpContextAccessor) : ICookieService
{
    public async Task SetAuthCookiesAsync(string username, string accessToken, string refreshToken)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim("RefreshToken", refreshToken),
            new Claim("uid", Guid.NewGuid().ToString())
        };

        var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        await httpContextAccessor.HttpContext!.SignInAsync(
            "Cookies",
            claimsPrincipal,
            new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            });

        httpContextAccessor.HttpContext!.Response.Cookies.Append("AccessToken", accessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddHours(1)
        });
    }

    public void RemoveAuthCookies()
    {
        httpContextAccessor.HttpContext!.SignOutAsync("Cookies").GetAwaiter().GetResult();
        httpContextAccessor.HttpContext!.Response.Cookies.Delete("AccessToken");
    }

    public string? GetRefreshToken()
    {
        return httpContextAccessor.HttpContext!.Request.Cookies["RefreshToken"];
    }
}