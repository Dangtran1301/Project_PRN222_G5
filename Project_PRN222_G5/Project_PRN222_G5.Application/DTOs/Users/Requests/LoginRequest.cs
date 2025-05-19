using Project_PRN222_G5.Application.Interfaces;
using Project_PRN222_G5.Domain.Entities.Users;
using System.ComponentModel.DataAnnotations;

namespace Project_PRN222_G5.Application.DTOs.Requests;

public class LoginRequest : IMapTo<User>
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    public User ToEntity() => new()
    {
        Username = Username,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(Password),
    };
}