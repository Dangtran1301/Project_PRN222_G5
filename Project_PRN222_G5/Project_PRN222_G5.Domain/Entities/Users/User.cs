using Project_PRN222_G5.Domain.Common;
using Project_PRN222_G5.Domain.Entities.Users.Enum;

namespace Project_PRN222_G5.Domain.Entities.Users;

public class User : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Username { get; set; }

    public string PasswordHash { get; set; }

    public string Email { get; set; }

    public Gender? Gender { get; set; }

    public DateTime? DayOfBirth { get; set; }

    public string? Avatar { get; set; }

    public UserStatus UserStatus { get; set; } = UserStatus.Active;

    public ICollection<Role> Roles { get; set; } = [];

    public ICollection<UserToken> UserTokens { get; set; } = [];

    public ICollection<UserResetPassword>? UserResetPasswords { get; set; } = [];
}