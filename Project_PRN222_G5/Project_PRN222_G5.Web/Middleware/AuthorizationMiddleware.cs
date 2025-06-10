using Project_PRN222_G5.DataAccess.Entities.Users.Enum;
using System.Security.Claims;

namespace Project_PRN222_G5.Web.Middleware;

public class AuthorizationMiddleware(RequestDelegate next, ILogger<AuthorizationMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path;

        if (path.StartsWithSegments("/Admin/**"))
        {
            if (!context.User.Identity?.IsAuthenticated ?? true)
            {
                logger.LogWarning("Unauthenticated access attempt to {Path}", path);
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Please log in.");
                return;
            }

            var roles = context.User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (!roles.Contains(nameof(Role.Admin)))
            {
                logger.LogWarning("Unauthorized access attempt to {Path} by user without Admin role", path);
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Forbidden: Admin access required.");
                return;
            }
        }

        if (path.StartsWithSegments("/Staff/**"))
        {
            if (!context.User.Identity?.IsAuthenticated ?? true)
            {
                logger.LogWarning("Unauthenticated access attempt to {Path}", path);
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Please log in.");
                return;
            }

            var roles = context.User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (!roles.Contains(nameof(Role.Staff)))
            {
                logger.LogWarning("Unauthorized access attempt to {Path} by user without Staff role", path);
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Forbidden: Staff access required.");
                return;
            }
        }

        await next(context);
    }
}

public static class AuthorizationMiddlewareExtensions
{
    public static IApplicationBuilder UseAuthorizationMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuthorizationMiddleware>();
    }
}