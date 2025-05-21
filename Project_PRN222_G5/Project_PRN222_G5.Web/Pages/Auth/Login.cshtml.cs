using Microsoft.AspNetCore.Mvc;
using Project_PRN222_G5.Application.DTOs.Users.Requests;
using Project_PRN222_G5.Application.Interfaces.Service.Identities;
using Project_PRN222_G5.Web.Pages.Shared;

namespace Project_PRN222_G5.Web.Pages.Auth
{
    [IgnoreAntiforgeryToken]
    public class LoginModel(IAuthService authService) : BasePageModel
    {
        [BindProperty]
        public LoginRequest Input { get; set; } = null!;

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                HandleModelStateErrors();
                return Page();
            }

            try
            {
                var response = await authService.LoginAsync(Input);
                TempData["SuccessMessage"] = "Login successfully!";
                Response.Cookies.Append("AccessToken", response.AccessToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddHours(1)
                });
                return RedirectToPage(PageRoutes.Users.Index);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return Page();
            }
        }
    }
}