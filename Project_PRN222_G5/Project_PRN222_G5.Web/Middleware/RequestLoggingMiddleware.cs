using static System.DateTime;

namespace Project_PRN222_G5.Web.Middleware
{
    public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            var startTime = UtcNow;
            try
            {
                await next(context);
            }
            finally
            {
                var elapsed = (UtcNow - startTime).TotalMilliseconds;
                var coloredTime = ColorMilliseconds(elapsed);
                var coloredStatus = ColorStatus(context.Response.StatusCode);
                var coloredMethod = ColorMethod(context.Request.Method);

                logger.LogInformation($"{startTime:MM/dd/yyyy HH:mm:ss} - " +
                                      $"Request: {coloredMethod} {context.Request.Path} " +
                                      $"by User: {context.User.Identity?.Name ?? "Anonymous"} " +
                                      $"took {coloredTime} Status: {coloredStatus}");
            }
        }

        private static string ColorStatus(int statusCode)
        {
            return statusCode switch
            {
                >= 200 and < 300 => $"\x1b[32m{statusCode}\x1b[0m",
                >= 300 and < 400 => $"\x1b[36m{statusCode}\x1b[0m",
                >= 400 and < 500 => $"\x1b[33m{statusCode}\x1b[0m",
                _ => $"\x1b[31m{statusCode}\x1b[0m"
            };
        }

        private static string ColorMilliseconds(double milliSeconds)
        {
            return milliSeconds switch
            {
                < 100 => $"\x1b[32m{milliSeconds}\x1b[0m ms",
                < 500 => $"\x1b[33m{milliSeconds}\x1b[0m ms",
                _ => $"\x1b[31m{milliSeconds}\x1b[0m ms"
            };
        }

        private static string ColorMethod(string method)
        {
            return method.ToUpper() switch
            {
                "GET" => $"\x1b[32m{method}\x1b[0m", // Blue
                "POST" => $"\x1b[35m{method}\x1b[0m", // Magenta
                "PUT" => $"\x1b[36m{method}\x1b[0m", // Cyan
                "DELETE" => $"\x1b[31m{method}\x1b[0m", // Red
                _ => $"\x1b[37m{method}\x1b[0m", // White/Default
            };
        }
    }
}