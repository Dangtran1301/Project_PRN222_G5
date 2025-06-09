using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project_PRN222_G5.BusinessLogic.Extensions;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Project_PRN222_G5.Web.Utilities;

public static class HtmlUtil
{
    public static string? FormatValue(object value)
    {
        return value switch
        {
            null => "—",
            DateTime dt => dt.ToString("dd/MM/yyyy"),
            bool b => b ? "✅" : "❌",
            Enum e => GetEnumDisplayName(e),
            IEnumerable list and not string => string.Join(", ", list.Cast<object>()),
            _ => value.ToString()
        };
    }

    public static string GetDisplayName(PropertyInfo property)
    {
        var displayAttr = property.GetCustomAttribute<DisplayAttribute>();
        return displayAttr?.Name ?? property.Name;
    }

    public static string? GetEnumDisplayName(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var displayAttr = field?.GetCustomAttribute<DisplayAttribute>();
        return displayAttr?.Name ?? value.ToString();
    }

    public static IHtmlContent GenerateInput(PropertyInfo prop, object model, Dictionary<string, List<SelectListItem>>? selectLists = null)
    {
        const string formControlClass = "form-control";
        var displayName = GetDisplayName(prop);
        var inputName = prop.Name;
        var value = prop.GetValue(model);
        var imageAttr = prop.GetCustomAttribute<ImageDisplayAttribute>();
        var inputId = $"input-{inputName}-{Guid.NewGuid():N}"; // Unique ID for JavaScript

        // Enum select list
        if (selectLists?.TryGetValue(inputName, out var options) == true)
        {
            return new HtmlString($@"
                <select name=""{inputName}"" class=""{formControlClass}"" id=""{inputId}"" asp-items=""@options""></select>
            ");
        }
        // DateTime or Nullable<DateTime>
        else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
        {
            var dateValue = value is DateTime date ? date.ToString("yyyy-MM-dd") : "";
            return new HtmlString($@"
                <input name=""{inputName}"" type=""date"" value=""{dateValue}"" class=""{formControlClass}"" id=""{inputId}"" />
            ");
        }
        // Password
        else if ("password".Contains(inputName.ToLower()))
        {
            return new HtmlString($@"
                <input name=""{inputName}"" type=""password"" value="""" class=""{formControlClass}"" id=""{inputId}"" />
            ");
        }
        // Boolean (checkbox)
        else if (prop.PropertyType == typeof(bool))
        {
            var isChecked = value is true ? "checked" : "";
            return new HtmlString($@"
                <input name=""{inputName}"" type=""checkbox"" value=""true"" class=""{formControlClass}"" id=""{inputId}"" {isChecked} />
                <input type=""hidden"" name=""{inputName}"" value=""false"" />
            ");
        }
        // File input with image preview
        else if (prop.PropertyType == typeof(IFormFile))
        {
            var props = model.GetType().GetProperties();
            var pathProp = props.FirstOrDefault(p => p.Name == $"{inputName}Path" || p.Name == "AvatarPath");
            var path = pathProp?.GetValue(model)?.ToString();
            var previewId = $"preview-{inputName}-{Guid.NewGuid():N}";
            var clearButtonId = $"clear-{inputName}-{Guid.NewGuid():N}";
            var previewImg = string.IsNullOrWhiteSpace(path)
                ? $@"<img id=""{previewId}"" style=""display:none; max-height:{imageAttr?.MaxHeight ?? 150}px;{(imageAttr?.CssClass ?? "")}"" class=""{imageAttr?.CssClass ?? "img-thumbnail"}"" alt=""Preview {displayName}"" />"
                : $@"<img id=""{previewId}"" src=""{path}"" style=""max-height:{imageAttr?.MaxHeight ?? 150}px;{(imageAttr?.CssClass ?? "")}"" class=""{imageAttr?.CssClass ?? "img-thumbnail"}"" alt=""Preview {displayName}"" />";

            var inputSection = $@"
        <input name=""{inputName}"" type=""file"" class=""{formControlClass}"" id=""{inputId}"" accept=""image/*"" onchange=""previewImage(this, '{previewId}')"" />
        <button type=""button"" id=""{clearButtonId}"" class=""btn btn-secondary mt-2"" onclick=""clearImage('{inputId}', '{previewId}')"" style=""{(string.IsNullOrWhiteSpace(path) ? "display:none;" : "")}"">Xóa ảnh</button>
    ";

            return new HtmlString($@"
        <div class=""mb-2"">{previewImg}</div>
        <div>{inputSection}</div>
    ");
        }
        // Default input
        else
        {
            return new HtmlString($@"
                <input name=""{inputName}"" class=""{formControlClass}"" value=""{value?.ToString() ?? ""}"" id=""{inputId}"" />
            ");
        }
    }

    public static IHtmlContent GenerateDisplay(PropertyInfo prop, object model)
    {
        var value = prop.GetValue(model);
        var imageAttr = prop.GetCustomAttribute<ImageDisplayAttribute>();
        var displayName = GetDisplayName(prop);

        if (imageAttr != null && value is string path && !string.IsNullOrWhiteSpace(path))
        {
            return new HtmlString(GetImageHtml(path, imageAttr, displayName));
        }

        return new HtmlString(FormatValue(value!));
    }

    private static string GetImageHtml(string path, ImageDisplayAttribute? imageAttr, string displayName)
    {
        var style = $"max-height:{imageAttr?.MaxHeight ?? 150}px;";
        if (imageAttr != null)
        {
            switch (imageAttr.Shape)
            {
                case ImageShape.Circle:
                    style += " border-radius:50%; object-fit:cover;";
                    break;
                case ImageShape.Square:
                    style += $" width:{imageAttr.MaxHeight}px; height:{imageAttr.MaxHeight}px; object-fit:cover;";
                    break;
                case ImageShape.Rectangle:
                default:
                    break;
            }
        }
        var cssClass = imageAttr?.CssClass ?? "img-thumbnail";
        var altText = imageAttr?.Alt ?? $"Preview of {displayName}";
        return $@"<img src=""{path}"" style=""{style}"" class=""{cssClass}"" alt=""{altText}"" />";
    }
}