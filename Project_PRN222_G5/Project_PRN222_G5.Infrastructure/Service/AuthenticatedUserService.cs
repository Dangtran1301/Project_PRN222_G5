using Microsoft.AspNetCore.Http;
using Project_PRN222_G5.Application.Interfaces.Service;
using System.Security.Claims;

namespace Project_PRN222_G5.Infrastructure.Service;

public class AuthenticatedUserService(IHttpContextAccessor httpContextAccessor) : IAuthenticatedUserService
{
    public string? UserId { get; } = httpContextAccessor.HttpContext?.User?.FindFirstValue("uid")!;
}