using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_PRN222_G5.Application.DTOs.Requests;
using Project_PRN222_G5.Application.DTOs.Responses;
using Project_PRN222_G5.Application.Interfaces;
using Project_PRN222_G5.Application.Mapper.Users;
using Project_PRN222_G5.Domain.Entities.Users.Enum;
using Project_PRN222_G5.Web.Utils;

namespace Project_PRN222_G5.Web.Pages.Users
{
    [Authorize(Roles = nameof(Role.Admin))]
    public class EditModel(IUserService userService) : PageModel
    {
        [BindProperty]
        public UpdateInfoUser Input { get; set; } = new();

        public new UserResponse User { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                Input.ToUpdateInfoUser(await userService.GetByIdAsync(id));
                return Page();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await userService.UpdateAsync(id, Input);
                return RedirectToPage(PageRoutes.Users.Index);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}