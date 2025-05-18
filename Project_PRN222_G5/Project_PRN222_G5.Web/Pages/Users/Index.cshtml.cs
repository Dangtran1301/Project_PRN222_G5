using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_PRN222_G5.Application.DTOs.Responses;
using Project_PRN222_G5.Application.Interfaces;
using Project_PRN222_G5.Domain.Entities.Users.Enum;

namespace Project_PRN222_G5.Web.Pages.Users
{
    [Authorize(Roles = nameof(Role.Admin))]
    public class IndexModel(IUserService userService) : PageModel
    {
        public IEnumerable<UserResponse> Users { get; set; } = new List<UserResponse>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }

        public async Task OnGetAsync(int page = 1)
        {
            if (page < 1) page = 1;
            CurrentPage = page;
            Users = await userService.GetPagedAsync(page, PageSize);
            TotalPages = (int)Math.Ceiling((double)TotalCount / PageSize);
        }
    }
}