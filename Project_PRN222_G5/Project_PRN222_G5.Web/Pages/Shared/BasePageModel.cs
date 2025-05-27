using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using Project_PRN222_G5.BusinessLogic.Exceptions;

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

    protected void HandleException(Exception ex)
    {
        if (ex is ValidationException validationEx)
        {
            var errors = new StringBuilder();
            foreach (var error in validationEx.Errors)
            {
                errors.AppendLine(string.Join(", ", error.Value));
            }
            ErrorMessage = errors.ToString().Trim();
        }
        else
        {
            ErrorMessage = $"An error occurred: {ex.Message}";
        }
    }
}