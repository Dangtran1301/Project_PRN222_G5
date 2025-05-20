using Project_PRN222_G5.Application.Interfaces.Mapping;
using Project_PRN222_G5.Domain.Entities.Users;
using Project_PRN222_G5.Domain.Entities.Users.Enum;
using System.ComponentModel.DataAnnotations;

namespace Project_PRN222_G5.Application.DTOs.Users.Requests;

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

    [FileExtensions(Extensions = "jgp,jpeg")]
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