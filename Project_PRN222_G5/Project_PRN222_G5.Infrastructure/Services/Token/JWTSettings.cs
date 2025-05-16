namespace Project_PRN222_G5.Infrastructure.Services.Token;

public class JWTSettings
{
    public string? SecretKey { get; set; }

    public string? ExpireTimeAccessToken { get; set; }

    public string? ExpireTimeRefreshToken { get; set; }
}