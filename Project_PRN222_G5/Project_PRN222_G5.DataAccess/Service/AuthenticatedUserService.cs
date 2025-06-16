using Project_PRN222_G5.DataAccess.Interfaces.Service;

namespace Project_PRN222_G5.DataAccess.Service;

public class AuthenticatedUserService : IAuthenticatedUserService
{
    public string UserId { get; private set; } = string.Empty;
    public string ClientIp { get; private set; } = string.Empty;

    public void SetUser(string userId, string clientIp)
    {
        UserId = userId;
        ClientIp = clientIp;
    }
}