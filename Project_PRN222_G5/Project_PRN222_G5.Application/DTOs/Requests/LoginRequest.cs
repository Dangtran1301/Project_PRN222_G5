using System.ComponentModel.DataAnnotations;

namespace Project_PRN222_G5.Application.DTOs.Requests;

public class LoginRequest
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}