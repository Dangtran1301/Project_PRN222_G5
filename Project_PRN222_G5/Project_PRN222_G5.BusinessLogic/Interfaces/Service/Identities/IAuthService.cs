using Project_PRN222_G5.BusinessLogic.DTOs.Users.Requests;
using Project_PRN222_G5.BusinessLogic.DTOs.Users.Responses;
using Project_PRN222_G5.DataAccess.Entities.Users;

namespace Project_PRN222_G5.BusinessLogic.Interfaces.Service.Identities;

public interface IAuthService : IGenericService<User, RegisterUserRequest, UpdateInfoUser, UserResponse>
{
    Task<LoginResponse> LoginAsync(LoginRequest loginRequest);

    Task LogoutAsync(Guid userId, string refreshToken);

    Task<LoginResponse> RefreshTokenAsync(RefreshTokenRequest request);

    Task<UserResponse> GetUserByUsernameAsync(string username);
}