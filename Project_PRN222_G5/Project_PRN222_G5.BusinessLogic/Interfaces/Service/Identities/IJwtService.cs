using Project_PRN222_G5.DataAccess.Entities.Users;
using System.Security.Claims;

namespace Project_PRN222_G5.BusinessLogic.Interfaces.Service.Identities;

public interface IJwtService
{
    string GenerateAccessToken(User user);

    ClaimsPrincipal? GetClaimsPrincipalFromExpiredToken(string token);
}