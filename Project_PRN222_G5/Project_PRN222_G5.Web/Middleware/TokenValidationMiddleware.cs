using Project_PRN222_G5.Domain.Entities.Users.Enum;

namespace Project_PRN222_G5.Web.Middleware
{
    public class TokenValidationMiddleware(RequestDelegate next, ILogger<TokenValidationMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            if (
                context.Request.Path.StartsWithSegments("/Auth/SignIn") ||
                context.Request.Path.StartsWithSegments("/Auth/SignUp") ||
                context.Request.Path.StartsWithSegments("/")
                )
            {
                await next(context);
                return;
            }

            var token = context.Request.Cookies["AccessToken"];
            if (string.IsNullOrEmpty(token) ||
                !context.User.Identity!.IsAuthenticated)
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized: No valid token found.");
                }
                else
                {
                    logger.LogWarning("Cannot set 401 response because response has already started for {Path}.", context.Request.Path);
                }
                return;
            }

            if (context.Request.Path.StartsWithSegments("/Users"))
            {
                var roles = context.User.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.Role).Select(c => c.Value).ToList();
                if (!roles.Contains(nameof(Role.Admin)))
                {
                    if (!context.Response.HasStarted)
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsync("Forbidden: Admin access required.");
                    }
                    else
                    {
                        logger.LogWarning("Cannot set 403 response because response has already started for {Path}.", context.Request.Path);
                    }
                    return;
                }
            }

            await next(context);
        }
    }
}