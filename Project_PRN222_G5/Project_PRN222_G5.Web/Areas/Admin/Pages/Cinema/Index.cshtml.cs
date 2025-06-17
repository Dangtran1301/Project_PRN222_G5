using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Cinema;
using Project_PRN222_G5.DataAccess.DTOs;
using Project_PRN222_G5.DataAccess.DTOs.Cinema.Response;
using Project_PRN222_G5.DataAccess.Entities.Users.Enum;
using Project_PRN222_G5.Web.Models;

namespace Project_PRN222_G5.Web.Areas.Admin.Pages.Cinema
{
    [Authorize(Roles = $"{nameof(Role.Admin)},{nameof(Role.Staff)}")]
    public class IndexModel(ICinemaService cinemaService) : BasePageModel
    {
        [BindProperty(SupportsGet = true)]
        public PagedRequest PagedRequest { get; set; } = new();

        public PaginationResponse<CinemaResponse> PaginationResponse { get; set; } = null!;

        public string[] DisplayProps { get; set; } = ["Name", "Address"];

        public async Task<IActionResult> OnGetAsync()
        {
            PaginationResponse = await cinemaService.GetPagedAsync(PagedRequest);
            return Page();
        }
    }
}