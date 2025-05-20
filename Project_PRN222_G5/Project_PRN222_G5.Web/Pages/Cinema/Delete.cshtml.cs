using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_PRN222_G5.Application.DTOs.Cinema.Response;
using Project_PRN222_G5.Application.Interfaces.Service;

namespace Project_PRN222_G5.Web.Pages.Cinema
{
    public class DeleteModel : PageModel
    {
        private readonly ICinemaService _cinemaService;

        public DeleteModel(ICinemaService cinemaService)
        {
            _cinemaService = cinemaService;
        }

        [BindProperty]
        public CinemaResponse Cinema { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null) return NotFound();

            try
            {
                var cinema = await _cinemaService.GetByIdAsync(id.Value);
                Cinema = cinema;
                return Page();
            }
            catch
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null) return NotFound();

            try
            {
                await _cinemaService.DeleteAsync(id.Value);
                return RedirectToPage("./Index");
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
