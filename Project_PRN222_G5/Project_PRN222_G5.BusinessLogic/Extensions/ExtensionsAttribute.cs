using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Project_PRN222_G5.BusinessLogic.Extensions;

public class ExtensionsAttribute(string[] extensions) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!extensions.Contains(extension))
            {
                return new ValidationResult($"Only the following extensions are allowed: {string.Join(", ", extensions)}");
            }
        }
        return ValidationResult.Success;
    }
}

[AttributeUsage(AttributeTargets.Property)]
public class ImageDisplayAttribute : Attribute
{
    public string Alt { get; set; } = string.Empty;
    public string CssClass { get; set; } = "img-thumbnail";
    public int MaxHeight { get; set; } = 150;
    public ImageShape Shape { get; set; } = ImageShape.Rectangle;

    public ImageDisplayAttribute()
    { }

    public ImageDisplayAttribute(string alt, string cssClass = "img-thumbnail", int maxHeight = 150, ImageShape shape = ImageShape.Rectangle)
    {
        Alt = alt;
        CssClass = cssClass;
        MaxHeight = maxHeight;
        Shape = shape;
    }
}

public static class AttributeExtensions
{
    public static ImageDisplayAttribute? GetImageDisplay(this PropertyInfo prop)
    {
        return prop.GetCustomAttributes(typeof(ImageDisplayAttribute), false)
            .FirstOrDefault() as ImageDisplayAttribute;
    }
}

public enum ImageShape
{
    Rectangle,
    Square,
    Circle
}