using Microsoft.AspNetCore.Mvc;
using Project_PRN222_G5.Application.Interfaces.Service;
using Project_PRN222_G5.Application.Interfaces.UnitOfWork;
using Project_PRN222_G5.Application.Interfaces.Validation;
using Project_PRN222_G5.Domain.Entities.Users;
using Project_PRN222_G5.Web.Pages.Shared;
using System.IdentityModel.Tokens.Jwt;

namespace Project_PRN222_G5.Web.Pages.Auth
{
    public class RefreshModel(IUnitOfWork unitOfWork, ITokenValidator tokenValidator, IJwtService jwtService)
        : BasePageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            var refreshToken = Request.Cookies["RefreshToken"];
            var accessToken = Request.Cookies["AccessToken"];

            if (string.IsNullOrWhiteSpace(refreshToken) || string.IsNullOrWhiteSpace(accessToken))
                return RedirectToPage("/Auth/Login");

            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token;

            try
            {
                token = handler.ReadJwtToken(accessToken);
            }
            catch
            {
                return RedirectToPage(PageRoutes.Auth.Login);
            }

            var userIdClaim = token.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            if (!Guid.TryParse(userIdClaim, out var userId))
                return RedirectToPage(PageRoutes.Auth.Login);

            var isValid = await tokenValidator.IsRefreshTokenValidAsync(userId, refreshToken);
            if (!isValid)
                return RedirectToPage(PageRoutes.Auth.Login);

            var user = await unitOfWork.Repository<User>().FindAsync(u => u.Id == userId);
            var userEntity = user.FirstOrDefault();
            if (userEntity is null)
                return RedirectToPage(PageRoutes.Auth.Login);

            var newAccessToken = jwtService.GenerateAccessToken(userEntity);

            Response.Cookies.Append("AccessToken", newAccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(15)
            });

            return Redirect(Request.Headers.Referer.ToString() ?? "/");
        }
    }
}
