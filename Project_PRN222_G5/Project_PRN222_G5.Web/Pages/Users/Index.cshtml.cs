using Microsoft.AspNetCore.Authorization;
using Project_PRN222_G5.BusinessLogic.DTOs;
using Project_PRN222_G5.BusinessLogic.DTOs.Users.Responses;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Identities;
using Project_PRN222_G5.DataAccess.Entities.Users.Enum;
using Project_PRN222_G5.Web.Pages.Shared;

namespace Project_PRN222_G5.Web.Pages.Users
{
    [Authorize(Roles = nameof(Role.Admin))]
    public class IndexModel(IAuthService authService) : BasePageModel
    {
        public PaginationResponse<UserResponse> PaginationResponse { get; set; } = null!;

        public async Task OnGetAsync(int pageNumber = 1)
        {
            if (pageNumber < 1) pageNumber = 1;
            const int pageSize = 10;
            PaginationResponse = await authService.GetPagedAsync(pageNumber, pageSize).ConfigureAwait(false);
        }
    }
}