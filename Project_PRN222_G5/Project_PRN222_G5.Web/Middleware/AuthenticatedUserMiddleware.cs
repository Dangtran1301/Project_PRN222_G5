using Project_PRN222_G5.DataAccess.Interfaces.Service;
using Project_PRN222_G5.DataAccess.Service;
using System.Security.Claims;

namespace Project_PRN222_G5.Web.Middleware;

public class AuthenticatedUserMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IAuthenticatedUserService userService)
    {
        var userId = context.User.FindFirstValue("uid") ?? string.Empty;
        var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? string.Empty;

        if (userService is AuthenticatedUserService serviceImpl)
        {
            serviceImpl.SetUser(userId, clientIp);
        }

        await next(context);
    }
}

public static class AuthenticatedUserMiddlewareExtensions
{
    public static IApplicationBuilder UseAuthenticatedUserMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuthenticatedUserMiddleware>();
    }
}