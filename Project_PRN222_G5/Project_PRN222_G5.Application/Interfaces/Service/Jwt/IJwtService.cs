using Project_PRN222_G5.DataAccess.Entities.Identities.Users;
using System.Security.Claims;

namespace Project_PRN222_G5.BusinessLogic.Interfaces.Service.Jwt;

public interface IJwtService
{
    string GenerateAccessToken(User user);

    ClaimsPrincipal? GetClaimsPrincipalFromExpiredToken(string token);
}