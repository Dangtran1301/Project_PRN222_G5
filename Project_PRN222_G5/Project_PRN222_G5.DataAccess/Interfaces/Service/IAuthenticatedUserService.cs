namespace Project_PRN222_G5.DataAccess.Interfaces.Service;

public interface IAuthenticatedUserService
{
    string UserId { get; }

    string ClientIp { get; }
}