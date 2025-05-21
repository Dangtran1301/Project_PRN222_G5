using Project_PRN222_G5.Application.Interfaces.Mapping;
using Project_PRN222_G5.Domain.Entities.Users;
using Project_PRN222_G5.Domain.Entities.Users.Enum;
using System.ComponentModel.DataAnnotations;

namespace Project_PRN222_G5.Application.DTOs.Users.Requests;

public class RegisterUserRequest : IMapTo<User>
{
    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(50, MinimumLength = 10, ErrorMessage = "Full name must not exceed 50 characters.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Username is required.")]
    [StringLength(50, ErrorMessage = "Username must not exceed 50 characters.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password, ErrorMessage = "Invalid password format.")]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$",
        ErrorMessage = "Password must be at least 8 characters long and include at least one uppercase letter, one lowercase letter, and one number.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirm password is required.")]
    [DataType(DataType.Password, ErrorMessage = "Invalid confirm password format.")]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Role is required.")]
    public string Role { get; set; } = string.Empty;

    public User ToEntity() => new()
    {
        FullName = FullName,
        Username = Username,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(Password),
        Email = Email,
        Role = Enum.Parse<Role>(Role, true),
    };
}