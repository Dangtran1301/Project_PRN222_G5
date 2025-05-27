using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project_PRN222_G5.DataAccess.Data;
using Project_PRN222_G5.DataAccess.Entities.Identities.Movie;
using Project_PRN222_G5.Application.DTOs.Cinema.Response;
using Project_PRN222_G5.Application.Interfaces.Service;
using Project_PRN222_G5.Application.Interfaces.Service.Identities;
using Project_PRN222_G5.Web.Pages.Shared;

namespace Project_PRN222_G5.Web.Pages.Cinema
{
    public class DetailsModel : BasePageModel
    {
        private readonly ICinemaService _cinemaService;

        public DetailsModel(ICinemaService cinemaService)
        {
            _cinemaService = cinemaService;
        }

        public CinemaResponse Cinema { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            try
            {
                Cinema = await _cinemaService.GetByIdAsync(id.Value);
                return Page();
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return RedirectToPage(PageRoutes.Static.NotFound);
            }
        }
    }
}
