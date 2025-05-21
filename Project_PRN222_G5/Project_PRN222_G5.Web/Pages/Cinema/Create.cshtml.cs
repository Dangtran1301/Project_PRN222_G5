using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_PRN222_G5.Application.DTOs.Cinema.Request;
using Project_PRN222_G5.Application.Interfaces.Service;

namespace Project_PRN222_G5.Web.Pages.Cinema;

public class CreateModel : PageModel
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
