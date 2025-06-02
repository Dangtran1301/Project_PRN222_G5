using Project_PRN222_G5.DataAccess.Entities.Bookings;
using Project_PRN222_G5.DataAccess.Entities.Common;
using Project_PRN222_G5.DataAccess.Entities.Users.Enum;

namespace Project_PRN222_G5.DataAccess.Entities.Users;

public class User : BaseEntity
{
    public string FullName { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public Gender Gender { get; set; } = Gender.Unknown;

    public DateTime? DayOfBirth { get; set; }

    public string Avatar { get; set; } = string.Empty;

    public UserStatus UserStatus { get; set; } = UserStatus.Active;

    public Role Role { get; set; } = Role.Customer;

    public ICollection<Booking> Bookings { get; set; } = [];

    public ICollection<UserToken> UserTokens { get; set; } = [];

    public ICollection<UserResetPassword>? UserResetPasswords { get; set; } = [];
}