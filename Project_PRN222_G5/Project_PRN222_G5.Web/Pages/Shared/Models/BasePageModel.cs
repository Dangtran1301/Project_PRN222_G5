using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_PRN222_G5.BusinessLogic.Exceptions;
using Project_PRN222_G5.Web.Utilities;
using System.Text;
using static System.String;

namespace Project_PRN222_G5.Web.Pages.Shared.Models;

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
                    if (!IsNullOrEmpty(error.ErrorMessage))
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
                errors.AppendLine(Join(", ", error.Value));
            }
            message = errors.ToString().Trim();
        }
        else
        {
            message = $"An error occurred: {ex.Message}";
        }

        return RedirectToPage(PageRoutes.Public.Error, new { message });
    }

    protected IActionResult HandleValidationExceptionOrThrow(Exception ex, string? returnPage = null)
    {
        if (ex is ValidationException vex)
        {
            foreach (var error in vex.Errors)
                foreach (var message in error.Value)
                    ModelState.AddModelError(error.Key, message);

            HandleModelStateErrors();
            return IsNullOrEmpty(returnPage) ? Page() : RedirectToPage(returnPage);
        }

        return HandleException(ex);
    }
}