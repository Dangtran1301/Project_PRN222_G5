using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_PRN222_G5.BusinessLogic.DTOs.Users.Requests;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Identities;
using Project_PRN222_G5.DataAccess.Entities.Identities.Users;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Project_PRN222_G5.Web.Pages.Shared;

namespace Project_PRN222_G5.Web.Pages.Auth
{
    [IgnoreAntiforgeryToken]
    public class RefreshModel(
        IAuthService authService,
        ICookieService cookieService,
        ILogger<RefreshModel> logger
        ) : BasePageModel
    {
        public async Task<IActionResult> OnPostAsync()
        {
            var accessToken = Request.Cookies["AccessToken"];
            var refreshToken = User.FindFirst("RefreshToken")?.Value;

            if (string.IsNullOrWhiteSpace(refreshToken) || string.IsNullOrWhiteSpace(accessToken))
            {
                logger.LogWarning("Missing refresh or access token in refresh request");
                return Unauthorized();
            }

            try
            {
                var userIdClaim = User.FindFirst("uid")?.Value;
                if (!Guid.TryParse(userIdClaim, out var userId))
                {
                    logger.LogWarning("Invalid user ID in refresh request");
                    return Unauthorized();
                }

                var request = new RefreshTokenRequest
                {
                    UserId = userId,
                    RefreshToken = refreshToken
                };

                var response = await authService.RefreshTokenAsync(request);
                await cookieService.SetAuthCookiesAsync(User.Identity!.Name!, response.AccessToken, response.RefreshToken);

                return new JsonResult(new { response.AccessToken });
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return Unauthorized();
            }
        }
    }
}