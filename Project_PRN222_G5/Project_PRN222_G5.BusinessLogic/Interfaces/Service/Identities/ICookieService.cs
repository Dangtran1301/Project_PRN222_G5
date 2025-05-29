namespace Project_PRN222_G5.BusinessLogic.Interfaces.Service.Identities;

public interface ICookieService
{
    Task SetAuthCookiesAsync(string username, string accessToken, string refreshToken);

    void RemoveAuthCookies();

    string? GetRefreshToken();
}