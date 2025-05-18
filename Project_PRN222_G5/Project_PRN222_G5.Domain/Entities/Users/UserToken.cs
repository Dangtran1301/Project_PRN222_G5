using Project_PRN222_G5.Domain.Common;

namespace Project_PRN222_G5.Domain.Entities.Users;

public class UserToken : BaseEntity
{
    public string RefreshToken { get; set; } = string.Empty;

    public string? ClientIp { get; set; }

    public bool IsBlocked { get; set; } = false;

    public DateTimeOffset ExpiredTime { get; set; }

    public Guid UserId { get; set; }

    public User? User { get; set; }
}