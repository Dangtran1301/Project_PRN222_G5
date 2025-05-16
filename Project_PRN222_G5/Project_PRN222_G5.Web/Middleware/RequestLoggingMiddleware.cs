namespace Project_PRN222_G5.Web.Middleware
{
    public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            var startTime = DateTime.UtcNow;
            try
            {
                await next(context);
            }
            finally
            {
                var elapsedMs = (DateTime.UtcNow - startTime).TotalSeconds;
                logger.LogInformation(
                    "Request: {Method} {Path} by User: {User} took {ElapsedMs}ms Status: {StatusCode}\n",
                    context.Request.Method,
                    context.Request.Path,
                    context.User.Identity?.Name ?? "Anonymous",
                    elapsedMs,
                    context.Response.StatusCode);
            }
        }
    }
}