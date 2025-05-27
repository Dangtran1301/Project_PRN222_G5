using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Project_PRN222_G5.DataAccess.Interfaces.Service;

namespace Project_PRN222_G5.DataAccess.Service;

public class AuthenticatedUserService(IHttpContextAccessor contextAccessor) : IAuthenticatedUserService
{
    public string UserId { get; } = contextAccessor.HttpContext?.User.FindFirstValue("uid") ?? string.Empty;
    public string ClientIp { get; } = contextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
}