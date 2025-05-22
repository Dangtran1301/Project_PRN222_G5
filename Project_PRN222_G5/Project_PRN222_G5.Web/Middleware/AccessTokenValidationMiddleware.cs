using Project_PRN222_G5.Application.Interfaces.Validation;
using Project_PRN222_G5.Web.Pages.Shared;

namespace Project_PRN222_G5.Web.Middleware;
public class AccessTokenValidationMiddleware(
    RequestDelegate next,
    ILogger<AccessTokenValidationMiddleware> logger)
{

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path;

        if (path.StartsWithSegments(PageRoutes.Auth.Login)
            || path.StartsWithSegments(PageRoutes.Auth.Logout)
            || path.StartsWithSegments(PageRoutes.Auth.Refresh))
        {
            await next(context);
            return;
        }

        if (!context.User.Identity?.IsAuthenticated ?? true)
        {
            context.Response.Redirect(PageRoutes.Auth.Refresh);
            return;
        }

        await next(context);
    }
}
