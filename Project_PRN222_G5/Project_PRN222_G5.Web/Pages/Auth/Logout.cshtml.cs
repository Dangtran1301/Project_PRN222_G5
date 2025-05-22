using Microsoft.AspNetCore.Mvc;
using Project_PRN222_G5.Application.Interfaces.Service.Identities;
using Project_PRN222_G5.Web.Pages.Shared;

namespace Project_PRN222_G5.Web.Pages.Auth
{
    public class LogoutModel(IAuthService authService) : BasePageModel
    {
        public async Task<IActionResult> OnPost()
        {
            var refreshToken = Request.Cookies["RefreshToken"];
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;

            if (Guid.TryParse(userIdClaim, out var userId) && !string.IsNullOrEmpty(refreshToken))
            {
                await authService.LogoutAsync(userId, refreshToken);
            }

            Response.Cookies.Delete("AccessToken");
            Response.Cookies.Delete("RefreshToken");

            return RedirectToPage(PageRoutes.Static.Home);
        }
    }
}