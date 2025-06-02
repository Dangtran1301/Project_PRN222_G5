using Microsoft.AspNetCore.Http;
using Project_PRN222_G5.DataAccess.Interfaces.Service;
using System.Security.Claims;

namespace Project_PRN222_G5.DataAccess.Service;

public class AuthenticatedUserService(IHttpContextAccessor contextAccessor) : IAuthenticatedUserService
{
    private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

    public string UserId => GetUserId();
    public string ClientIp => GetClientIp();

    private string GetUserId()
    {
        return _contextAccessor.HttpContext?.User.FindFirstValue("uid") ?? string.Empty;
    }

    private string GetClientIp()
    {
        return _contextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
    }
}