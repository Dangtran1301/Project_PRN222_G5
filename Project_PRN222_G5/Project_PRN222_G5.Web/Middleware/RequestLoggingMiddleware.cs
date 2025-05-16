using Microsoft.AspNetCore.Http;
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

                var log = $"{startTime:MM/dd/yyyy HH:mm:ss} - " +
                          $"Request: {coloredMethod} {context.Request.Path} " +
                          $"by User: {context.User.Identity?.Name ?? "Anonymous"} " +
                          $"took {coloredTime} Status: {coloredStatus}";

                logger.LogInformation(log);
            }
        }

        private static string ColorStatus(int statusCode)
        {
            if (statusCode >= 200 && statusCode < 300) return $"\x1b[32m{statusCode}\x1b[0m"; // Green
            if (statusCode >= 300 && statusCode < 400) return $"\x1b[36m{statusCode}\x1b[0m"; // Cyan
            if (statusCode >= 400 && statusCode < 500) return $"\x1b[33m{statusCode}\x1b[0m"; // Yellow
            return $"\x1b[31m{statusCode}\x1b[0m"; // Red
        }

        private static string ColorMilliseconds(double milliSeconds)
        {
            if (milliSeconds < 100) return $"\x1b[32m{milliSeconds}\x1b[0m ms";
            if (milliSeconds < 500) return $"\x1b[33m{milliSeconds}\x1b[0m ms";
            return $"\x1b[31m{milliSeconds}\x1b[0m ms";
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