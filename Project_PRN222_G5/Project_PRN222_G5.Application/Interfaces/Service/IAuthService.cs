using Project_PRN222_G5.Application.DTOs.Requests;
using Project_PRN222_G5.Application.DTOs.Users.Requests;
using Project_PRN222_G5.Application.DTOs.Users.Responses;
using Project_PRN222_G5.Domain.Entities.Users;

namespace Project_PRN222_G5.Application.Interfaces.Service;

public interface IAuthService : IGenericService<User, RegisterUserRequest, UpdateInfoUser, UserResponse>
{
    Task<LoginResponse> LoginAsync(LoginRequest loginRequest);

    Task<UserResponse> RegisterUserAsync(RegisterUserRequest request);

    Task<User> GetByUsernameAsync(string username);

    Task<User> GetByEmailAsync(string email);
}