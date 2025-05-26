using Project_PRN222_G5.Infrastructure.Entities.Users.Enum;

namespace Project_PRN222_G5.Web.Middleware;

public class AuthorizationMiddleware(RequestDelegate next, ILogger<AuthorizationMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path;
        if (path.StartsWithSegments("/Users"))
        {
            var roles = context.User.Claims
                .Where(c => c.Type == System.Security.Claims.ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (!roles.Contains(nameof(Role.Admin)))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Forbidden: Admin access required.");
                return;
            }
        }

        await next(context);
    }
}