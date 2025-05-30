using Microsoft.AspNetCore.Mvc;
using Project_PRN222_G5.BusinessLogic.DTOs.Cinema.Response;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Cinema;
using Project_PRN222_G5.Web.Pages.Shared.Models;

namespace Project_PRN222_G5.Web.Pages.Cinema
{
    public class DetailsModel(ICinemaService cinemaService) : BasePageModel
    {
        public CinemaResponse Cinema { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            try
            {
                Cinema = await cinemaService.GetByIdAsync(id.Value);
                return Page();
            }
            catch (Exception ex)
            {
                return HandleValidationExceptionOrThrow(ex);
            }
        }
    }
}