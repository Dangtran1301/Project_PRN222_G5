using Project_PRN222_G5.Infrastructure.DTOs.Users.Requests;
using Project_PRN222_G5.Infrastructure.DTOs.Users.Responses;
using Project_PRN222_G5.Infrastructure.Entities.Users;

namespace Project_PRN222_G5.Application.Mapper.Users;

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
        Gender = entity.Gender.ToString(),
        Role = entity.Role.ToString()
    };

    public static void UpdateEntity(this User entity, UpdateInfoUser request)
    {
        var updateEntity = request.ToEntity();
        entity.FullName = updateEntity.FullName;
        entity.Gender = updateEntity.Gender;
    }

    public static void ToUpdateInfoUser(this UpdateInfoUser request, UserResponse userResponse)
    {
        request.Fullname = userResponse.FullName;
        request.Gender = userResponse.Gender;
        request.DayOfBirth = userResponse.DayOfBirth;
        request.PhoneNumber = userResponse.PhoneNumber;
        request.Avatar = userResponse.Avatar;
    }
}