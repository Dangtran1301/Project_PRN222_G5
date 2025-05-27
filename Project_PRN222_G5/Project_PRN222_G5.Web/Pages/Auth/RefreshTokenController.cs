using Microsoft.AspNetCore.Mvc;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Jwt;
using Project_PRN222_G5.BusinessLogic.Interfaces.Validation;
using Project_PRN222_G5.DataAccess.Entities.Identities.Users;
using Project_PRN222_G5.DataAccess.Interfaces.UnitOfWork;

namespace Project_PRN222_G5.Web.Pages.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefreshTokenController(
        IUnitOfWork unitOfWork,
        ITokenValidator tokenValidator,
        IJwtService jwtService,
        ILogger logger) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Refresh()
        {
            var refreshToken = Request.Cookies["RefreshToken"];
            var accessToken = Request.Cookies["AccessToken"];

            if (string.IsNullOrWhiteSpace(refreshToken) || string.IsNullOrWhiteSpace(accessToken))
                return Unauthorized();

            var principal = jwtService.GetClaimsPrincipalFromExpiredToken(accessToken);
            var userIdClaim = principal?.FindFirst("uid")?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var isValid = await tokenValidator.IsRefreshTokenValidAsync(userId, refreshToken);
            if (!isValid)
                return Unauthorized();

            var user = await unitOfWork.Repository<User>().FindAsync(u => u.Id == userId);
            var userEntity = user.FirstOrDefault();
            if (userEntity == null)
                return Unauthorized();

            var newAccessToken = jwtService.GenerateAccessToken(userEntity);

            Response.Cookies.Append("AccessToken", newAccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddSeconds(3)
            });

            return Ok(new { AccessToken = newAccessToken });
        }
    }
}