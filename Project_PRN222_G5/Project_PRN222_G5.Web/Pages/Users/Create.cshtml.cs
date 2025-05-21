using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project_PRN222_G5.Application.DTOs.Users.Requests;
using Project_PRN222_G5.Application.Interfaces.Service.Identities;
using Project_PRN222_G5.Domain.Entities.Users.Enum;
using Project_PRN222_G5.Web.Pages.Shared;

namespace Project_PRN222_G5.Web.Pages.Users
{
    [Authorize(Roles = nameof(Role.Admin))]
    public class CreateModel(IAuthService authService) : BasePageModel
    {
        [BindProperty]
        public RegisterUserRequest Input { get; set; } = new();

        [ViewData]
        public List<SelectListItem> Roles { get; set; } = Enum.GetValues(typeof(Role))
            .Cast<Role>()
            .Where(r => r != Role.Admin)
            .Select(r => new SelectListItem { Value = r.ToString(), Text = r.ToString() })
            .ToList();

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
                await authService.RegisterUserAsync(Input);
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