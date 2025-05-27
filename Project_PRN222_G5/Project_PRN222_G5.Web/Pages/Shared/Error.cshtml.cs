using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Project_PRN222_G5.Web.Pages.Shared
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        public string? ErrorMessage { get; set; }

        public void OnGet(string? message)
        {
            ErrorMessage = message ?? "An unknown error occurred.";
        }
    }
}