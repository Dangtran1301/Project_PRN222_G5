using Microsoft.AspNetCore.Http;
using Project_PRN222_G5.DataAccess.Entities.Users.Enum;
using Project_PRN222_G5.DataAccess.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Project_PRN222_G5.DataAccess.DTOs.Users.Requests;

public class UpdateInfoUser
{
    [Required] public Guid Id { get; set; } = Guid.Empty;

    [Required(ErrorMessage = "Full name is required")]
    public string FullName { get; set; } = string.Empty;

    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    [DataType(DataType.Date)] public DateTime? DayOfBirth { get; set; } = default;

    [EnumDataType(typeof(Gender))]
    public string Gender { get; set; } = string.Empty;

    [Extensions([".jpg", ".jpeg", ".png"])]
    [ImageDisplay(MaxHeight = 100, Shape = ImageShape.Circle, CssClass = "img-circle", Alt = "User Avatar")]
    public IFormFile? Avatar { get; set; } = default;
}