using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_PRN222_G5.Application.DTOs;
using Project_PRN222_G5.Application.Interfaces.Service;
using Project_PRN222_G5.Domain.Entities.Users.Enum;

namespace Project_PRN222_G5.Web.Pages.Users
{
    [Authorize(Roles = nameof(Role.Admin))]
    public class IndexModel(IAuthService authService) : PageModel
    {
        public PagedResponse Response { get; set; } = new();

        public async Task OnGetAsync(int page = 1)
        {
            if (page < 1) page = 1;
            const int pageSize = 10;
            Response = await authService.GetPagedAsync(page, pageSize);
        }
    }
}