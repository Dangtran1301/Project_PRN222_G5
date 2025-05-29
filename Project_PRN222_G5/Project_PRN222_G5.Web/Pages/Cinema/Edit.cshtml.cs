using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project_PRN222_G5.DataAccess.Data;
using Project_PRN222_G5.Web.Utilities;

using Project_PRN222_G5.BusinessLogic.DTOs.Cinema.Request;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Cinema;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Identities;
using Project_PRN222_G5.Web.Pages.Shared;

namespace Project_PRN222_G5.Web.Pages.Cinema
{
    public class EditModel : BasePageModel
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
            catch (Exception ex)
            {
                HandleException(ex);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                HandleModelStateErrors();
                CinemaId = id;
                return Page();
            }

            try
            {
                await _cinemaService.UpdateAsync(id, CinemaDto);
                return RedirectToPage(PageRoutes.Cinema.Index);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                CinemaId = id;
                return Page();
            }
        }
    }
}
