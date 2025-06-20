﻿using Project_PRN222_G5.BusinessLogic.Interfaces.Mapping;
using Project_PRN222_G5.DataAccess.Entities.Users;
using Project_PRN222_G5.DataAccess.Entities.Users.Enum;
using System.ComponentModel.DataAnnotations;

namespace Project_PRN222_G5.BusinessLogic.DTOs.Users.Requests;

public class UpdateInfoUser : IMapTo<User>
{
    [Required(ErrorMessage = "Full name is required")]
    public string Fullname { get; set; } = string.Empty;

    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime? DayOfBirth { get; set; }

    [EnumDataType(typeof(Gender))]
    public string Gender { get; set; } = string.Empty;

    [FileExtensions(Extensions = "jpg,jpeg,png")]
    public string Avatar { get; set; } = string.Empty;

    public User ToEntity() => new()
    {
        FullName = Fullname,
        PhoneNumber = PhoneNumber,
        DayOfBirth = DayOfBirth,
        Gender = Enum.Parse<Gender>(Gender, true),
        Avatar = Avatar,
    };
}