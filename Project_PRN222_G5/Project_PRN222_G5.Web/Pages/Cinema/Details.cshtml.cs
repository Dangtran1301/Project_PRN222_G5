using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_PRN222_G5.Application.DTOs.Cinema.Response;
using Project_PRN222_G5.Application.Interfaces.Service;

namespace Project_PRN222_G5.Web.Pages.Cinema
{
    public class DetailsModel : PageModel
    {
        private readonly ICinemaService _cinemaService;

        public DetailsModel(ICinemaService cinemaService)
        {
            _cinemaService = cinemaService;
        }

        public CinemaResponse Cinema { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null) return NotFound();

            try
            {
                Cinema = await _cinemaService.GetByIdAsync(id.Value);
                return Page();
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
