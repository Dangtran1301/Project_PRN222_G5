using Microsoft.AspNetCore.Mvc;
using Project_PRN222_G5.BusinessLogic.DTOs.Cinema.Request;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Cinema;
using Project_PRN222_G5.Web.Pages.Shared;
using Project_PRN222_G5.Web.Utilities;

namespace Project_PRN222_G5.Web.Pages.Cinema
{
    public class CreateModel : BasePageModel
    {
        private readonly ICinemaService _cinemaService;

        public CreateModel(ICinemaService cinemaService)
        {
            _cinemaService = cinemaService;
        }

        [BindProperty]
        public CreateCinemaDto CinemaDto { get; set; } = new();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                HandleModelStateErrors();
                return Page();
            }

            try
            {
                await _cinemaService.CreateAsync(CinemaDto);
                return RedirectToPage(PageRoutes.Cinema.Index);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return Page();
            }
        }
    }
}
