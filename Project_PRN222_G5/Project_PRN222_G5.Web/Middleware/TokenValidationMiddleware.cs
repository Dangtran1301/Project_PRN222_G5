using Project_PRN222_G5.Application.Interfaces.UnitOfWork;
using Project_PRN222_G5.Domain.Entities.Users;
using Project_PRN222_G5.Domain.Entities.Users.Enum;
using Project_PRN222_G5.Web.Pages.Shared;

namespace Project_PRN222_G5.Web.Middleware
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TokenValidationMiddleware> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public TokenValidationMiddleware(
            RequestDelegate next,
            ILogger<TokenValidationMiddleware> logger,
            IUnitOfWork unitOfWork)
        {
            _next = next;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (
             context.Request.Path.StartsWithSegments(PageRoutes.Auth.Login) ||
             context.Request.Path.StartsWithSegments(PageRoutes.Auth.Register) ||
             context.Request.Path.StartsWithSegments("")
            )
            {
                await _next(context);
                return;
            }

            var accessToken = context.Request.Cookies["AccessToken"];
            var refreshToken = context.Request.Cookies["RefreshToken"];
            if (string.IsNullOrEmpty(accessToken) || !context.User.Identity!.IsAuthenticated)
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized: No valid token found.");
                }
                else
                {
                    _logger.LogWarning("Cannot set 401 response because response has already started for {Path}.", context.Request.Path);
                }
                return;
            }

            var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Invalid user id in token.");
                return;
            }

            var userToken = (await _unitOfWork.Repository<UserToken>()
                .FindAsync(t =>
                    t.UserId == userId && t.RefreshToken == refreshToken
                                       && t.ExpiredTime > DateTimeOffset.UtcNow))
                .FirstOrDefault();

            if (userToken == null)
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized: Token expired or invalid.");
                }
                else
                {
                    _logger.LogWarning("Cannot set 401 response because response has already started for {Path}.",
                        context.Request.Path);
                }
                return;
            }

            if (context.Request.Path.StartsWithSegments("/Users"))
            {
                var roles = context.User.Claims
                    .Where(c => c.Type == System.Security.Claims.ClaimTypes.Role)
                    .Select(c => c.Value)
                    .ToList();
                if (!roles.Contains(nameof(Role.Admin)))
                {
                    if (!context.Response.HasStarted)
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsync("Forbidden: Admin access required.");
                    }
                    else
                    {
                        _logger.LogWarning("Cannot set 403 response because response has already started for {Path}.",
                            context.Request.Path);
                    }
                    return;
                }
            }

            await _next(context);
        }
    }
}