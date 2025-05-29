using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_PRN222_G5.BusinessLogic.DTOs;
using Project_PRN222_G5.BusinessLogic.DTOs.Cinema.Response;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Cinema;
using Project_PRN222_G5.DataAccess.Entities.Users.Enum;
using Project_PRN222_G5.Web.Pages.Shared.Models;

namespace Project_PRN222_G5.Web.Pages.Cinema
{
    [Authorize(Roles = nameof(Role.Admin))]
    public class IndexModel : BasePageModel
    {
        private readonly ICinemaService _cinemaService;

        public IndexModel(ICinemaService cinemaService)
        {
            _cinemaService = cinemaService;
        }

        [BindProperty(SupportsGet = true)]
        public PagedRequest PagedRequest { get; set; } = new();

        public PaginationResponse<CinemaResponse> PaginationResponse { get; set; } = null!;

        public string[] DisplayProps { get; set; } = ["Name", "Address"];

        public async Task<IActionResult> OnGetAsync()
        {
            PaginationResponse = await _cinemaService.GetPagedAsync(PagedRequest);
            return Page();
        }
    }
}
