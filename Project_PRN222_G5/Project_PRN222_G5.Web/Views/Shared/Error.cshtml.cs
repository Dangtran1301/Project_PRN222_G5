using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Project_PRN222_G5.Web.Views.Shared
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        public string? ErrorMessage { get; set; }

        public void OnGet()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            ErrorMessage = exceptionFeature?.Error.Message ?? "An unknown error occurred.";
        }
    }
}