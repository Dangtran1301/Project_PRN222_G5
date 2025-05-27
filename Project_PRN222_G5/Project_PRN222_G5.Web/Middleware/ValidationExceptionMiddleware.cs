using System.Text.Json;
using Project_PRN222_G5.BusinessLogic.Exceptions;

namespace Project_PRN222_G5.Web.Middleware;

public class ValidationExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            if (context.Request.Path.StartsWithSegments(""))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    errors = ex.Errors
                }));
            }
            else
            {
                throw;
            }
        }
    }
}