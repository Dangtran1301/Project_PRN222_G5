using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_PRN222_G5.Application.DTOs.Cinema.Response;
using Project_PRN222_G5.Application.Interfaces.Service.Identities;

namespace Project_PRN222_G5.Web.Pages.Cinema
{
    public class IndexModel : PageModel
    {
        private readonly ICinemaService _cinemaService;

        public IndexModel(ICinemaService cinemaService)
        {
            _cinemaService = cinemaService;
        }

        public IList<CinemaResponse> Cinemas { get; set; } = new List<CinemaResponse>();

        public async Task OnGetAsync()
        {
            var cinemas = await _cinemaService.GetAllAsync();
            Cinemas = cinemas.ToList();
        }
    }
}
