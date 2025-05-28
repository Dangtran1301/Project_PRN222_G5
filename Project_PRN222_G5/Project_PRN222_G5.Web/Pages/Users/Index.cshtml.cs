using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_PRN222_G5.BusinessLogic.DTOs;
using Project_PRN222_G5.BusinessLogic.DTOs.Users.Responses;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Identities;
using Project_PRN222_G5.DataAccess.Entities.Users.Enum;
using Project_PRN222_G5.Web.Pages.Shared;

namespace Project_PRN222_G5.Web.Pages.Users;

[Authorize(Roles = nameof(Role.Admin))]
public class IndexModel(IAuthService authService) : BasePageModel
{
    public PaginationResponse<UserResponse> PaginationResponse { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int? pageNumber)
    {
        const int pageSize = 10;
        var currentPage = pageNumber.GetValueOrDefault(1);
        if (currentPage < 1) currentPage = 1;

        PaginationResponse = await authService.GetPagedAsync(currentPage, pageSize);
        return Page();
    }
}