using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Project_PRN222_G5.Infrastructure.Interfaces.Service;

namespace Project_PRN222_G5.Infrastructure.Service;

public class AuthenticatedUserService(IHttpContextAccessor contextAccessor) : IAuthenticatedUserService
{
    public string UserId { get; } = contextAccessor.HttpContext?.User.FindFirstValue("uid") ?? string.Empty;
    public string ClientIp { get; } = contextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
}