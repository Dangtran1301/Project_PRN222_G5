using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Cinema;
using Project_PRN222_G5.DataAccess.DTOs.Cinema.Request;
using Project_PRN222_G5.DataAccess.Entities.Users.Enum;
using Project_PRN222_G5.Web.Models;
using Project_PRN222_G5.Web.Utilities;

namespace Project_PRN222_G5.Web.Areas.Admin.Pages.Cinema
{
    [Authorize(Roles = nameof(Role.Admin))]
    public class CreateModel(ICinemaService cinemaService) : BasePageModel
    {
        [BindProperty]
        public CreateCinemaDto CinemaDto { get; set; } = new();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                HandleModelStateErrors();
                return Page();
            }
            try
            {
                await cinemaService.CreateAsync(CinemaDto, cancellationToken);
                return RedirectToPage(PageRoutes.Cinema.Index);
            }
            catch (Exception e)
            {
                return HandleValidationExceptionOrThrow(e);
            }
        }
    }
}