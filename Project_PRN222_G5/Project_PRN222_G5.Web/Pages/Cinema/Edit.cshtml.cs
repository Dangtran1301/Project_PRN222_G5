using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_PRN222_G5.Application.DTOs.Cinema.Request;
using Project_PRN222_G5.Application.Interfaces.Service;

namespace Project_PRN222_G5.Web.Pages.Cinema
{
    public class EditModel : PageModel
    {
        private readonly ICinemaService _cinemaService;

        public EditModel(ICinemaService cinemaService)
        {
            _cinemaService = cinemaService;
        }

        [BindProperty]
        public UpdateCinemaDto CinemaDto { get; set; } = default!;

        public Guid CinemaId { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null) return NotFound();

            try
            {
                var cinema = await _cinemaService.GetByIdAsync(id.Value);
                if (cinema == null) return NotFound();

                CinemaId = id.Value;
                CinemaDto = new UpdateCinemaDto
                {
                    Id = id.Value,
                    Name = cinema.Name,
                    Address = cinema.Address
                };

                return Page();
            }
            catch
            {
                return NotFound();
            }
        }


        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                CinemaId = id;
                return Page();
            }

            try
            {
                await _cinemaService.UpdateAsync(id, CinemaDto);
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                CinemaId = id;
                return Page();
            }
        }
    }
}
