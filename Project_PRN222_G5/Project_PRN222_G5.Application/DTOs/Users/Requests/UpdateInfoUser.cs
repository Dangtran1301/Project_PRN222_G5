using Project_PRN222_G5.Application.Interfaces;
using Project_PRN222_G5.Domain.Entities.Users;
using Project_PRN222_G5.Domain.Entities.Users.Enum;

namespace Project_PRN222_G5.Application.DTOs.Requests;

public class UpdateInfoUser : IMapTo<User>
{
    public string Fullname { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime? DayOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
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