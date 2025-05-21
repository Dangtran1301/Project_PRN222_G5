using Project_PRN222_G5.Application.Exceptions;
using Project_PRN222_G5.Web.Pages.Shared;
using System.Text.Json;

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
            if (context.Request.Path.StartsWithSegments(PageRoutes.Static.Home))
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