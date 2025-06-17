using Project_PRN222_G5.BusinessLogic.Interfaces.Service;
using Project_PRN222_G5.DataAccess.DTOs.Users.Requests;
using Project_PRN222_G5.DataAccess.DTOs.Users.Responses;
using Project_PRN222_G5.DataAccess.Entities.Users;
using Project_PRN222_G5.DataAccess.Entities.Users.Enum;

namespace Project_PRN222_G5.BusinessLogic.Mapper.Users;

public static class UserMapper
{
    public static UserResponse ToResponse(this User entity) => new()
    {
        Id = entity.Id,
        FullName = entity.FullName,
        Username = entity.Username,
        Email = entity.Email,
        DayOfBirth = entity.DayOfBirth,
        PhoneNumber = entity.PhoneNumber,
        Avatar = string.IsNullOrEmpty(entity.Avatar)
            ? "/images/default-avatar.jpg"
            : entity.Avatar,
        Gender = entity.Gender.ToString(),
        Role = entity.Role
    };

    public static void UpdateEntity(this User entity, UpdateInfoUser request, IMediaService mediaService)
    {
        entity.FullName = request.FullName;
        entity.Gender = Enum.Parse<Gender>(request.Gender, true);
        entity.DayOfBirth = request.DayOfBirth;
        entity.PhoneNumber = request.PhoneNumber;
    }

    public static void ToUpdateInfoUser(this UpdateInfoUser request, UserResponse userResponse)
    {
        request.FullName = userResponse.FullName;
        request.Gender = userResponse.Gender;
        request.DayOfBirth = userResponse.DayOfBirth;
        request.PhoneNumber = userResponse.PhoneNumber;
    }
}