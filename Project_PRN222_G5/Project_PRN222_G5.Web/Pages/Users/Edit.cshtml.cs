using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_PRN222_G5.BusinessLogic.DTOs.Users.Requests;
using Project_PRN222_G5.BusinessLogic.DTOs.Users.Responses;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Identities;
using Project_PRN222_G5.BusinessLogic.Mapper.Users;
using Project_PRN222_G5.DataAccess.Entities.Users.Enum;
using Project_PRN222_G5.Web.Pages.Shared.Models;
using Project_PRN222_G5.Web.Utilities;

namespace Project_PRN222_G5.Web.Pages.Users
{
    [Authorize(Roles = nameof(Role.Admin))]
    public class EditModel(IAuthService authService) : BasePageModel
    {
        [BindProperty]
        public UpdateInfoUser Input { get; set; } = new();

        public new UserResponse User { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Input.ToUpdateInfoUser(await authService.GetByIdAsync(id));
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                HandleModelStateErrors();
                return Page();
            }

            await authService.UpdateAsync(id, Input);
            return RedirectToPage(PageRoutes.Users.Index);
        }
    }
}