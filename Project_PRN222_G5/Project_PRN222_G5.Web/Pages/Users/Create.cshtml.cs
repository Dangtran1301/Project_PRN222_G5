using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project_PRN222_G5.Application.DTOs.Requests;
using Project_PRN222_G5.Application.Interfaces;
using Project_PRN222_G5.Domain.Entities.Users.Enum;

namespace Project_PRN222_G5.Web.Pages.Users
{
    [Authorize(Roles = nameof(Role.Admin))]
    public class CreateModel(IUserService userService) : PageModel
    {
        [BindProperty]
        public RegisterUserRequest Input { get; set; } = new();

        public IActionResult OnGet()
        {
            ViewData["Roles"] = new List<SelectListItem>
            {
                new() { Value = nameof(Role.Author), Text = nameof(Role.Author) },
                new() { Value = nameof(Role.Reader), Text = nameof(Role.Reader) }
            };
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
                await userService.CreateAsync(Input);
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}