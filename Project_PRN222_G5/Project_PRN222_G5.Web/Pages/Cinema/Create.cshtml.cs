using Microsoft.AspNetCore.Mvc;
using Project_PRN222_G5.Application.DTOs.Cinema.Request;
using Project_PRN222_G5.Application.Interfaces.Service.Identities;
using Project_PRN222_G5.Web.Pages.Shared;

namespace Project_PRN222_G5.Web.Pages.Cinema
{
    public class CreateModel(ICinemaService cinemaService) : BasePageModel
    {
        _cinemaService = cinemaService;
    }

    [BindProperty]
    public CreateCinemaDto CinemaDto { get; set; } = new();

    public IActionResult OnGet()
    {
        return Page();
    }

        [BindProperty]
        public CreateCinemaDto Input { get; set; } = null!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

        try
        {
            await _cinemaService.CreateAsync(CinemaDto);
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
