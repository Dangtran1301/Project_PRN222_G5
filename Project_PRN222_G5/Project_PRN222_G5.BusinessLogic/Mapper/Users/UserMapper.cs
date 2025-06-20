﻿using Project_PRN222_G5.BusinessLogic.DTOs.Users.Requests;
using Project_PRN222_G5.BusinessLogic.DTOs.Users.Responses;
using Project_PRN222_G5.DataAccess.Entities.Users;

namespace Project_PRN222_G5.BusinessLogic.Mapper.Users;

public static class UserMapper
{
    public static UserResponse ToResponse(this User entity) => new()
    {
        Id = entity.Id,
        FullName = entity.FullName,
        Username = entity.Username,
        Email = entity.Email,
        DayOfBirth = entity.DayOfBirth?.ToString("dd-MM-yyyy") ?? "N/A",
        PhoneNumber = entity.PhoneNumber,
        Gender = entity.Gender.ToString(),
        Role = entity.Role.ToString()
    };

    public static void UpdateEntity(this User entity, UpdateInfoUser request)
    {
        var updateEntity = request.ToEntity();
        entity.FullName = updateEntity.FullName;
        entity.Gender = updateEntity.Gender;
        entity.DayOfBirth = updateEntity.DayOfBirth;
        entity.PhoneNumber = updateEntity.PhoneNumber;
        entity.Avatar = updateEntity.Avatar;
    }

    public static void ToUpdateInfoUser(this UpdateInfoUser request, UserResponse userResponse)
    {
        request.Fullname = userResponse.FullName;
        request.Gender = userResponse.Gender;
        request.DayOfBirth = DateTime.Parse(userResponse.DayOfBirth.ToString());
        request.PhoneNumber = userResponse.PhoneNumber;
        request.Avatar = userResponse.Avatar;
    }
}