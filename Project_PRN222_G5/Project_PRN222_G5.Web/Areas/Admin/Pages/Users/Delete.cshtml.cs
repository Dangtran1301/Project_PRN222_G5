using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_PRN222_G5.BusinessLogic.DTOs.Users.Responses;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Identities;
using Project_PRN222_G5.DataAccess.Entities.Users.Enum;
using Project_PRN222_G5.Web.Models;
using Project_PRN222_G5.Web.Utilities;

namespace Project_PRN222_G5.Web.Areas.Admin.Pages.Users
{
    [Authorize(Roles = nameof(Role.Admin))]
    public class DeleteModel(IAuthService authService) : BasePageModel
    {
        public new UserResponse User { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            User = await authService.GetByIdAsync(id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            await authService.DeleteAsync(id);
            return RedirectToPage(PageRoutes.Users.Index);
        }
    }
}