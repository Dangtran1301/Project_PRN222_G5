using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_PRN222_G5.Application.DTOs.Requests;
using Project_PRN222_G5.Application.Interfaces;

namespace Project_PRN222_G5.Web.Pages.Account
{
    [IgnoreAntiforgeryToken]
    public class SignInModel : PageModel
    {
        private readonly IAuthService _authService;
        private readonly ILogger<SignInModel> _logger;

        public SignInModel(IAuthService authService, ILogger<SignInModel> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [BindProperty]
        public LoginRequest Input { get; set; } = default!;

        public string? ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for SignIn: {Errors}",
                    string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                return Page();
            }

            try
            {
                var response = await _authService.LoginAsync(Input);
                Response.Cookies.Append("AccessToken", response.AccessToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(30)
                });
                _logger.LogInformation("User {Username} signed in successfully.", Input.Username);
                return RedirectToPage("/Users/Index");
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("SignIn failed for {Username}: {Message}", Input.Username, ex.Message);
                ErrorMessage = "Invalid username or password.";
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during SignIn for {Username}.", Input.Username);
                ErrorMessage = "An unexpected error occurred. Please try again.";
                return Page();
            }
        }
    }
}