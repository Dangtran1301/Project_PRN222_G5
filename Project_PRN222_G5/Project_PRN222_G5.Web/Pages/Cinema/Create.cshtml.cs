using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_PRN222_G5.Application.DTOs.Cinema.Request;
using Project_PRN222_G5.Application.Interfaces.Service;
using Project_PRN222_G5.Web.Utils;

namespace Project_PRN222_G5.Web.Pages.Cinema
{
    public class CreateModel(ICinemaService cinemaService) : PageModel
    {
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

            await cinemaService.CreateAsync(Input);
            return RedirectToPage(PageRoutes.Cinema.Index);
        }
    }
}