using Project_PRN222_G5.Web.Utilities;
using System.Diagnostics;

namespace Project_PRN222_G5.Web.Middleware;

public class LoggerMiddleware(
    RequestDelegate next,
    ILogger<LoggerMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var currentUser = ClaimsUtil.GetUsername(context.User);
        var method = context.Request.Method;
        var path = context.Request.Path;
        var queryString = context.Request.QueryString.ToString();

        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            logger.LogError(ex,
                "An error occurred while handling request: {Method} {Path}{QueryString} by User: {Username} in {ElapsedMs}ms",
                method, path, queryString, currentUser, stopwatch.ElapsedMilliseconds);
            throw;
        }
        finally
        {
            stopwatch.Stop();
            var statusCode = context.Response.StatusCode;
            var elapsedMs = stopwatch.ElapsedMilliseconds;
            var logMessage = "Request: {Method} {Path}{QueryString} with status code {StatusCode} by User: {Username} in {ElapsedMs}ms";

            if (elapsedMs > 500)
            {
                logger.LogWarning(logMessage, method, path, queryString, statusCode, currentUser, elapsedMs);
            }
            else
            {
                logger.LogInformation(logMessage, method, path, queryString, statusCode, currentUser, elapsedMs);
            }
        }
    }
}

public static class LoggerMiddlewareExtensions
{
    public static IApplicationBuilder UseLoggerMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LoggerMiddleware>();
    }
}