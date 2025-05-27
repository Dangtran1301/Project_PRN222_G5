using Project_PRN222_G5.BusinessLogic.DTOs.Users.Requests;
using Project_PRN222_G5.BusinessLogic.DTOs.Users.Responses;
using Project_PRN222_G5.DataAccess.Entities.Identities.Users;

namespace Project_PRN222_G5.BusinessLogic.Interfaces.Service.Identities;

public interface IAuthService : IGenericService<User, RegisterUserRequest, UpdateInfoUser, UserResponse>
{
    Task<LoginResponse> LoginAsync(LoginRequest loginRequest);

    Task<UserResponse> RegisterUserAsync(RegisterUserRequest request);

    Task LogoutAsync(Guid userId, string refreshToken);

    Task<LoginResponse> RefreshTokenAsync(RefreshTokenRequest request);
}