using Microsoft.AspNetCore.Mvc;
using Project_PRN222_G5.BusinessLogic.DTOs.Cinema.Request;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Cinema;
using Project_PRN222_G5.Web.Pages.Shared.Models;
using Project_PRN222_G5.Web.Utilities;

namespace Project_PRN222_G5.Web.Pages.Cinema
{
    public class CreateModel(ICinemaService cinemaService) : BasePageModel
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
                HandleModelStateErrors();
                return Page();
            }

            await cinemaService.CreateAsync(Input);
            return RedirectToPage(PageRoutes.Cinema.Index);
        }
    }
}