using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_PRN222_G5.Application.DTOs.Requests;
using Project_PRN222_G5.Application.Interfaces;
using Project_PRN222_G5.Web.Utils;

namespace Project_PRN222_G5.Web.Pages.Auth
{
    [IgnoreAntiforgeryToken]
    public class LoginModel(IAuthService authService, ILogger<LoginModel> logger) : PageModel
    {

        [BindProperty]
        public LoginRequest Input { get; set; } = null!;

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
                var response = await authService.LoginAsync(Input);
                Response.Cookies.Append("AccessToken", response.AccessToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(30)
                });
                return RedirectToPage(PageRoutes.Users.Index);
            }
            catch (UnauthorizedAccessException)
            {
                return Page();
            }
            catch (Exception)
            {
                return Page();
            }
        }
    }
}