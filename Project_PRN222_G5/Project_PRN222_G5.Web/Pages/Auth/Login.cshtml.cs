using Microsoft.AspNetCore.Mvc;
using Project_PRN222_G5.BusinessLogic.DTOs.Users.Requests;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Identities;
using Project_PRN222_G5.Web.Pages.Shared.Models;
using Project_PRN222_G5.Web.Utilities;

namespace Project_PRN222_G5.Web.Pages.Auth
{
    [IgnoreAntiforgeryToken]
    public class LoginModel(IAuthService authService, ICookieService cookieService) : BasePageModel
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
                await cookieService.SetAuthCookiesAsync(Input.Username, response.AccessToken, response.RefreshToken);

                TempData["SuccessMessage"] = "Login successfully!";
                return RedirectToPage(PageRoutes.Users.Index);
            }
            catch (Exception ex)
            {
                return HandleValidationExceptionOrThrow(ex);
            }
        }

        public IActionResult OnPostLogoutAsync()
        {
            cookieService.RemoveAuthCookies();
            TempData["SuccessMessage"] = "Logged out successfully!";
            return RedirectToPage(PageRoutes.Auth.Login);
        }
    }
}