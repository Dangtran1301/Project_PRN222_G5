using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_PRN222_G5.BusinessLogic.Exceptions;
using Project_PRN222_G5.Web.Utilities;
using System.Text;

namespace Project_PRN222_G5.Web.Pages.Shared;

public abstract class BasePageModel : PageModel
{
    public string? ErrorMessage { get; set; }

    protected void HandleModelStateErrors()
    {
        if (!ModelState.IsValid)
        {
            var errors = new StringBuilder();
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    if (!string.IsNullOrEmpty(error.ErrorMessage))
                    {
                        errors.AppendLine(error.ErrorMessage);
                    }
                    if (error.Exception != null)
                    {
                        errors.AppendLine(error.Exception.Message);
                    }
                }
            }
            ErrorMessage = errors.ToString().Trim();
        }
    }

    protected IActionResult HandleException(Exception ex)
    {
        string message;
        if (ex is ValidationException validationEx)
        {
            var errors = new StringBuilder();
            foreach (var error in validationEx.Errors)
            {
                errors.AppendLine(string.Join(", ", error.Value));
            }
            message = errors.ToString().Trim();
        }
        else
        {
            message = $"An error occurred: {ex.Message}";
        }

        return RedirectToPage(PageRoutes.Public.Error, new { message });
    }
}