using Project_PRN222_G5.Domain.Common;
using Project_PRN222_G5.Domain.Entities.Users.Enum;

namespace Project_PRN222_G5.Domain.Entities.Users;

public class User : BaseEntity
{
    public string FullName { get; set; } = string.Empty;

    public string Username { get; set; }

    public string PasswordHash { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public Gender Gender { get; set; } = Gender.Unknown;

    public DateTime? DayOfBirth { get; set; }

    public string Avatar { get; set; } = string.Empty;

    public UserStatus UserStatus { get; set; } = UserStatus.Active;

    public Role Role { get; set; } = Role.Customer;

    public ICollection<Booking.Booking> Bookings { get; set; } = [];

    public ICollection<UserToken> UserTokens { get; set; } = [];

    public ICollection<UserResetPassword>? UserResetPasswords { get; set; } = [];
}