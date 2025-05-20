using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_PRN222_G5.Application.DTOs.Users.Responses;
using Project_PRN222_G5.Application.Interfaces.Service;
using Project_PRN222_G5.Domain.Entities.Users.Enum;
using Project_PRN222_G5.Web.Utils;

namespace Project_PRN222_G5.Web.Pages.Users
{
    [Authorize(Roles = nameof(Role.Admin))]
    public class DeleteModel(IAuthService authService) : PageModel
    {
        public new UserResponse User { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                User = await authService.GetByIdAsync(id);
                return Page();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            try
            {
                await authService.DeleteAsync(id);
                return RedirectToPage(PageRoutes.Users.Index);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}