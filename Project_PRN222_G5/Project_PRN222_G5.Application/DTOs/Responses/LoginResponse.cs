namespace Project_PRN222_G5.Application.DTOs.Responses;

public class LoginResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}