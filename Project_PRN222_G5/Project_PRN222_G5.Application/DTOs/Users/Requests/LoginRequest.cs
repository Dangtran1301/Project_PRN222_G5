using Project_PRN222_G5.BusinessLogic.Interfaces.Mapping;
using Project_PRN222_G5.DataAccess.Entities.Identities.Users;
using System.ComponentModel.DataAnnotations;

namespace Project_PRN222_G5.BusinessLogic.DTOs.Users.Requests;

public class LoginRequest : IMapTo<User>
{
    [Required(ErrorMessage = "Username is required.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$",
        ErrorMessage = "Password must be at least 6 characters long and include at least one uppercase letter," +
        "one lowercase letter, and one number.")]
    public string Password { get; set; } = string.Empty;

    public User ToEntity() => new()
    {
        Username = Username,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(Password),
    };
}