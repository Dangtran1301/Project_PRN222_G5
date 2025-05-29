using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_PRN222_G5.BusinessLogic.DTOs.Cinema.Response;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Cinema;
using Project_PRN222_G5.BusinessLogic.DTOs;
using Project_PRN222_G5.DataAccess.Entities.Users.Enum;
using Project_PRN222_G5.Web.Pages.Shared;

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

        public PagedResponse Response { get; set; } = new();

        // Nếu dùng @page "{pageNumber:int?}" trong .cshtml
        public async Task<IActionResult> OnGetAsync(int? pageNumber)
        {
            int page = pageNumber ?? 1;
            const int pageSize = 10;

            try
            {
                Response = await _cinemaService.GetPagedAsync(page, pageSize);
                return Page();
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return Page();
            }
        }

    }
}
