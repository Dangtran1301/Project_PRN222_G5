using Project_PRN222_G5.Domain.Entities.Users;
using System.Security.Claims;

namespace Project_PRN222_G5.Application.Interfaces.Service;

public interface IJwtService
{
    string GenerateAccessToken(User user);

    ClaimsPrincipal? GetClaimsPrincipalFromExpiredToken(string token);
}