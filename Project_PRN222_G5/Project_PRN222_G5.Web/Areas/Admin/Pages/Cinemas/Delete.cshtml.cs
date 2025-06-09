using Microsoft.AspNetCore.Mvc;
using Project_PRN222_G5.BusinessLogic.DTOs.Cinema.Response;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Cinema;
using Project_PRN222_G5.Web.Models;
using Project_PRN222_G5.Web.Utilities;

namespace Project_PRN222_G5.Web.Areas.Admin.Pages.Cinemas
{
    public class DeleteModel(ICinemaService cinemaService) : BasePageModel
    {
        [BindProperty]
        public CinemaResponse Cinema { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            try
            {
                var cinema = await cinemaService.GetByIdAsync(id.Value);
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
                await cinemaService.DeleteAsync(id.Value);
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