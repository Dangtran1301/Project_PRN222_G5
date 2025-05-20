using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_PRN222_G5.Application.DTOs.Requests;
using Project_PRN222_G5.Application.Interfaces.Service;
using Project_PRN222_G5.Web.Utils;

namespace Project_PRN222_G5.Web.Pages.Auth
{
    [IgnoreAntiforgeryToken]
    public class RegisterModel(IAuthService authService, ILogger<RegisterModel> logger) : PageModel
    {
        [BindProperty]
        public RegisterUserRequest Input { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await authService.RegisterUserAsync(Input);
                return RedirectToPage(PageRoutes.Auth.Login);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error during Register for {Username}.", Input.Username);
                ErrorMessage = "An unexpected error occurred. Please try again.";
                return Page();
            }
        }
    }
}