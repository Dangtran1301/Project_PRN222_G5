using Project_PRN222_G5.DataAccess.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Project_PRN222_G5.DataAccess.DTOs.Users.Requests;

public class ResetPasswordRequest
{
    [Required(ErrorMessage = "Old password is required.")]
    public string OldPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "New password is required.")]
    [MinLength(8, ErrorMessage = "New password must be at least 8 characters long.")]
    [RegularExpression(@"^(?=.*\d)(?=.*[\W_]).{8,}$",
        ErrorMessage = "New password must contain at least one number and one special character.")]
    [NotEqualTo(nameof(OldPassword), ErrorMessage = "New password must be different from old password.")]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirm password is required.")]
    [Compare(nameof(NewPassword), ErrorMessage = "Confirm password does not match the new password.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}