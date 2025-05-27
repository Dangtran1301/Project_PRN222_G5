using Project_PRN222_G5.DataAccess.Entities.Common;

namespace Project_PRN222_G5.DataAccess.Entities.Identities.Users;

public class UserToken : BaseEntity
{
    public string RefreshToken { get; set; } = string.Empty;

    public string? ClientIp { get; set; }

    public DateTimeOffset ExpiredTime { get; set; }

    public Guid UserId { get; set; }

    public User? User { get; set; }
}