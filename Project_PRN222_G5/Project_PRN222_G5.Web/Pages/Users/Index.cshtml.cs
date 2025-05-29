using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_PRN222_G5.BusinessLogic.DTOs;
using Project_PRN222_G5.BusinessLogic.DTOs.Users.Responses;
using Project_PRN222_G5.BusinessLogic.Extensions;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Identities;
using Project_PRN222_G5.DataAccess.Entities.Users;
using Project_PRN222_G5.DataAccess.Entities.Users.Enum;
using Project_PRN222_G5.Web.Pages.Shared.Models;

namespace Project_PRN222_G5.Web.Pages.Users;

[Authorize(Roles = nameof(Role.Admin))]
public class IndexModel(IAuthService authService) : BasePageModel
{
    [BindProperty(SupportsGet = true)]
    public PagedRequest PagedRequest { get; set; } = new();
    public PaginationResponse<UserResponse> PaginationResponse { get; set; } = null!;

    public string[] DisplayProps { get; set; } = ["FullName", "Username", "Email", "DayOfBirth", "PhoneNumber", "Gender"];

    public async Task<IActionResult> OnGetAsync()
    {
        PaginationResponse = await authService.GetPagedAsync(PagedRequest);

        return Page();
    }
}