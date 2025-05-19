using Project_PRN222_G5.Application.DTOs.Requests;
using Project_PRN222_G5.Application.DTOs.Users.Responses;
using Project_PRN222_G5.Domain.Entities.Users;

namespace Project_PRN222_G5.Application.Interfaces;

public interface IUserService : IGenericService<User, RegisterUserRequest, UpdateInfoUser, UserResponse>
{
    Task<UserResponse> RegisterUserAsync(RegisterUserRequest request);

    Task<IEnumerable<UserResponse>> GetPagedAsync(int page, int pageSize);

    Task<User> GetByUsernameAsync(string username);

    Task<User> GetByEmailAsync(string email);
}