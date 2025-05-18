using Project_PRN222_G5.Domain.Common;

namespace Project_PRN222_G5.Domain.Entities.Users;

public class UserResetPassword : BaseEntity
{
    public string Token { get; set; } = string.Empty;

    public DateTimeOffset Expiry { get; set; }

    public Guid UserId { get; set; }

    public User? User { get; set; }
}