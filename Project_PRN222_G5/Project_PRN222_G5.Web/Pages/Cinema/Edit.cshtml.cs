using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_PRN222_G5.BusinessLogic.DTOs.Cinema.Request;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Cinema;
using Project_PRN222_G5.DataAccess.Entities.Users.Enum;
using Project_PRN222_G5.Web.Pages.Shared.Models;
using Project_PRN222_G5.Web.Utilities;

namespace Project_PRN222_G5.Web.Pages.Cinema
{
    [Authorize(Roles = $"{nameof(Role.Admin)},{nameof(Role.Staff)}")]
    public class EditModel(ICinemaService cinemaService) : BasePageModel
    {
        [BindProperty]
        public UpdateCinemaDto CinemaDto { get; set; } = null!;

        public Guid CinemaId { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            try
            {
                var cinema = await cinemaService.GetByIdAsync(id.Value);

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
                return HandleValidationExceptionOrThrow(ex, PageRoutes.Cinema.Edit);
            }
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                HandleModelStateErrors();
                return Page();
            }
            try
            {
                await cinemaService.UpdateAsync(id, CinemaDto);
                return RedirectToPage(PageRoutes.Cinema.Index);
            }
            catch (Exception e)
            {
                return HandleValidationExceptionOrThrow(e);
            }
        }
    }
}