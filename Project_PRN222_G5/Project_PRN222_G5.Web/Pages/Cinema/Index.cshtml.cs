using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_PRN222_G5.BusinessLogic.DTOs.Cinema.Response;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Identities;

namespace Project_PRN222_G5.Web.Pages.Cinema
{
    public class IndexModel(ICinemaService cinemaService) : PageModel
    {
        public IEnumerable<CinemaResponse> List { get; set; } = [];
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }

        public async Task OnGetAsync(int page = 1)
        {
            if (page < 1) page = 1;
            CurrentPage = page;
            List = await cinemaService.GetAllAsync();
            TotalPages = (int)Math.Ceiling((double)TotalCount / PageSize);
        }
    }
}