using Project_PRN222_G5.Web.Utilities;

namespace Project_PRN222_G5.Web.Middleware;

public class LoggerMiddleware(
    RequestDelegate next,
    ILogger<LoggerMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var currentUser = ClaimsUtil.GetUsername(context.User);
        var method = context.Request.Method;
        var path = context.Request.Path;
        var statusCode = context.Response.StatusCode;
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "An error occurred while handling request: {Method} {Path} by User: {Username}",
                method,
                path,
                currentUser);
            throw;
        }

        logger.LogInformation(
            "Request: {Method} {Path} with status code {StatusCode} by User: {Username}",
            method,
            path,
            statusCode,
            currentUser);
    }
}

public static class LoggerMiddlewareExtensions
{
    public static IApplicationBuilder UseLoggerMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LoggerMiddleware>();
    }
}