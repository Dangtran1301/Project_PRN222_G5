using Microsoft.AspNetCore.Mvc;
using Project_PRN222_G5.BusinessLogic.DTOs.Users.Responses;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Identities;
using Project_PRN222_G5.Web.Pages.Shared.Models;

namespace Project_PRN222_G5.Web.Pages.Users
{
    public class DetailsModel(IAuthService authService) : BasePageModel
    {
        public new UserResponse User { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
                User = await authService.GetByIdAsync(id);
                return Page();

        }
    }
}