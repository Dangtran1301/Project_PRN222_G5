using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project_PRN222_G5.DataAccess.Data;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Cinema;
using Project_PRN222_G5.BusinessLogic.DTOs.Cinema.Response;
using Project_PRN222_G5.Web.Utilities;

namespace Project_PRN222_G5.Web.Pages.Cinema
{
    public class DeleteModel : BasePageModel
    {
        private readonly ICinemaService _cinemaService;

        public DeleteModel(ICinemaService cinemaService)
        {
            _cinemaService = cinemaService;
        }

        [BindProperty]
        public CinemaResponse Cinema { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            try
            {
                var cinema = await _cinemaService.GetByIdAsync(id.Value);
                Cinema = cinema;
                return Page();
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return RedirectToPage(PageRoutes.Cinema.Index);
            }
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            try
            {
                await _cinemaService.DeleteAsync(id.Value);
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
