using Project_PRN222_G5.Application.Interfaces;
using Project_PRN222_G5.Domain.Entities.Users;
using Project_PRN222_G5.Domain.Entities.Users.Enum;

namespace Project_PRN222_G5.Application.DTOs.Requests;

public class RegisterUserRequest : IMapTo<User>
{
    public string FullName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

    public User ToEntity() => new()
    {
        FullName = FullName,
        Username = Username,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(Password),
        Email = Email,
        Role = Enum.Parse<Role>(Role, true),
        CreatedAt = DateTimeOffset.UtcNow,
        CreatedBy = "System"
    };
}