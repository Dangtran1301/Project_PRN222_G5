using Project_PRN222_G5.DataAccess.Entities.Users.Enum;
using Project_PRN222_G5.DataAccess.Extensions;

namespace Project_PRN222_G5.DataAccess.DTOs.Users.Responses;

public class UserResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime? DayOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    [ImageDisplay("User avatar", "img-thumbnail shadow", 120, Shape = ImageShape.Circle)]
    public string Avatar { get; set; } = string.Empty;

    public Role Role { get; set; }

    public DateTime CreatedAt = default;
}