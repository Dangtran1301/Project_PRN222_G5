namespace Project_PRN222_G5.Web.Middleware;

public class RequestTimeoutMiddleware
{
    private readonly RequestDelegate _next;
    private readonly TimeSpan _timeout;

    public RequestTimeoutMiddleware(RequestDelegate next, TimeSpan timeout)
    {
        _next = next;
        _timeout = timeout;
    }

    public async Task Invoke(HttpContext context)
    {
        using var cts = new CancellationTokenSource(_timeout);
        var timeoutTask = Task.Delay(_timeout, cts.Token);
        var processingTask = _next(context);

        var completedTask = await Task.WhenAny(processingTask, timeoutTask);
        if (completedTask == timeoutTask)
        {
            context.Response.StatusCode = StatusCodes.Status504GatewayTimeout;
            await context.Response.WriteAsync("Request timed out.", cancellationToken: cts.Token);
        }
        else
        {
            await cts.CancelAsync();
            await processingTask;
        }
    }
}

public static class RequestTimeoutMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestTimeoutMiddleware(this IApplicationBuilder builder, TimeSpan timeout) =>
        builder.UseMiddleware<RequestTimeoutMiddleware>(timeout);
}