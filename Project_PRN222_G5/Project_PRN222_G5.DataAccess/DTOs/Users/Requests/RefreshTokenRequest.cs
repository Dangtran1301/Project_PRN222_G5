namespace Project_PRN222_G5.DataAccess.DTOs.Users.Requests;

public class RefreshTokenRequest
{
    public Guid UserId { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
}