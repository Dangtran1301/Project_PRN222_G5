using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_PRN222_G5.Application.DTOs.Requests;
using Project_PRN222_G5.Application.Interfaces;

namespace Project_PRN222_G5.Web.Pages.Auth
{
    [IgnoreAntiforgeryToken]
    public class SignUpModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly ILogger<SignUpModel> _logger;

        public SignUpModel(IUserService userService, ILogger<SignUpModel> logger)
        {
            _userService = userService;
            _logger = logger;
        }

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
                _logger.LogWarning("Invalid model state for SignUp: {Errors}",
                    string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                return Page();
            }

            try
            {
                await _userService.RegisterUserAsync(Input);
                _logger.LogInformation("User {Username} signed up successfully.", Input.Username);
                return RedirectToPage("/Auth/SignIn");
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation failed for SignUp: {Errors}", string.Join(", ", ex.Errors.Select(e => e.ErrorMessage)));
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError($"Input.{error.PropertyName}", error.ErrorMessage);
                }
                return Page();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("SignUp failed for {Username}: {Message}", Input.Username, ex.Message);
                ErrorMessage = ex.Message;
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during SignUp for {Username}.", Input.Username);
                ErrorMessage = "An unexpected error occurred. Please try again.";
                return Page();
            }
        }
    }
}