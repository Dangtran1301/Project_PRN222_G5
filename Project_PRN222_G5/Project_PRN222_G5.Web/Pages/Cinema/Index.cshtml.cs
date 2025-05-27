using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_PRN222_G5.BusinessLogic.DTOs.Cinema.Response;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Identities;
using Microsoft.AspNetCore.Mvc;
using Project_PRN222_G5.Application.DTOs;
using Project_PRN222_G5.Application.Interfaces.Service.Identities;
using Project_PRN222_G5.Web.Pages.Shared;

namespace Project_PRN222_G5.Web.Pages.Cinema
{
    public class IndexModel : BasePageModel
    {
        private readonly ICinemaService _cinemaService;

        public IndexModel(ICinemaService cinemaService)
        {
            _cinemaService = cinemaService;
        }

        public PagedResponse Response { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int page = 1)
        {
            if (page < 1) page = 1;
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
