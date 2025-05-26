using System.Security.Claims;
using Project_PRN222_G5.Infrastructure.Entities.Users;

namespace Project_PRN222_G5.Application.Interfaces.Service;

public interface IJwtService
{
    string GenerateAccessToken(User user);

    ClaimsPrincipal? GetClaimsPrincipalFromExpiredToken(string token);
}