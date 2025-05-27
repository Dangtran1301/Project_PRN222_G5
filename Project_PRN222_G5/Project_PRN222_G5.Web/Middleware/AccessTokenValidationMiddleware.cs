using Project_PRN222_G5.Web.Pages.Shared;

namespace Project_PRN222_G5.Web.Middleware;

public class AccessTokenValidationMiddleware(RequestDelegate next, ILogger<AccessTokenValidationMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path;

        if (path.StartsWithSegments(PageRoutes.Auth.Login)
            || path.StartsWithSegments(PageRoutes.Auth.Register)
            || path.StartsWithSegments(PageRoutes.Auth.Refresh))
        {
            logger.LogInformation("Bypassing token validation for auth route: {Path}", path);
            await next(context);
            return;
        }

        if (!context.User.Identity?.IsAuthenticated ?? true)
        {
            var refreshToken = context.Request.Cookies["RefreshToken"];
            var accessToken = context.Request.Cookies["AccessToken"];

            if (!string.IsNullOrWhiteSpace(refreshToken) && !string.IsNullOrWhiteSpace(accessToken))
            {
                try
                {
                    using var client = new HttpClient();
                    var response = await client.PostAsync("/api/auth/refresh", null);
                    if (response.IsSuccessStatusCode)
                    {
                        logger.LogInformation("Token refresh successful for request: {Path}", path);
                        await next(context);
                        return;
                    }
                    logger.LogWarning("Token refresh failed for request: {Path}", path);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error during token refresh for request: {Path}", path);
                }
            }

            logger.LogWarning("Redirecting to login from: {Path}", path);
            context.Response.Redirect(PageRoutes.Auth.Login);
            return;
        }

        await next(context);
    }
}

public static class AccessTokenValidationMiddlewareExtensions
{
    public static IApplicationBuilder UseAccessTokenValidationMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AccessTokenValidationMiddleware>();
    }
}