using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;

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
        ErrorMessage = ex.Message;
    }
}
