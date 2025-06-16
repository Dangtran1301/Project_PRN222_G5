using Project_PRN222_G5.DataAccess.DTOs.Users.Requests;
using Project_PRN222_G5.DataAccess.DTOs.Users.Responses;
using Project_PRN222_G5.DataAccess.Entities.Users;

namespace Project_PRN222_G5.BusinessLogic.Interfaces.Service.Identities;

public interface IUserService : IGenericService<User, RegisterUserRequest, UpdateInfoUser, UserResponse>
{
    Task<UserResponse> GetUserInfoById(Guid id);

    Task<bool> ResetPassword(Guid id, ResetPasswordRequest request);
}