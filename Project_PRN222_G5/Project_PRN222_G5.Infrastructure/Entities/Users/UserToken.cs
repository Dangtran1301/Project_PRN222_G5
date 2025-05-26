using Project_PRN222_G5.Infrastructure.Entities.Common;

namespace Project_PRN222_G5.Infrastructure.Entities.Users;

public class UserToken : BaseEntity
{
    public string RefreshToken { get; set; } = string.Empty;

    public string? ClientIp { get; set; }

    public DateTimeOffset ExpiredTime { get; set; }

    public Guid UserId { get; set; }

    public User? User { get; set; }
}