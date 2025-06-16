using Project_PRN222_G5.DataAccess.DTOs.Users.Responses;

namespace Project_PRN222_G5.BusinessLogic.Interfaces.Service.Identities;

public interface ICookieService
{
    Task SetAuthCookiesAsync(UserResponse user, string accessToken, string refreshToken);

    Task RemoveAuthCookiesAsync();

    string? GetRefreshToken();
}