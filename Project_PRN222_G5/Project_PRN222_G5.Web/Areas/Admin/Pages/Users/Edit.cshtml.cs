using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project_PRN222_G5.BusinessLogic.DTOs.Users.Requests;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Identities;
using Project_PRN222_G5.BusinessLogic.Mapper.Users;
using Project_PRN222_G5.DataAccess.Entities.Users.Enum;
using Project_PRN222_G5.Web.Models;
using Project_PRN222_G5.Web.Utilities;

namespace Project_PRN222_G5.Web.Areas.Admin.Pages.Users
{
    [Authorize(Roles = nameof(Role.Admin))]
    public class EditModel(IAuthService authService) : BasePageModel
    {
        [BindProperty]
        public UpdateInfoUser Input { get; set; } = new();

        [ViewData]
        public List<SelectListItem> Genders { get; set; } = [.. Enum.GetValues(typeof(Gender))
            .Cast<Gender>()
            .Where(g=>g != Gender.Unknown)
            .Select(r => new SelectListItem { Value = r.ToString(), Text = r.ToString() })];

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