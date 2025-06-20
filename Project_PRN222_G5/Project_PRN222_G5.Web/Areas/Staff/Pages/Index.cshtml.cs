using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Project_PRN222_G5.Web.Areas.Staff.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            ViewData["Title"] = "Dashboard";

            ViewData["DailyTicketSales"] = new
            {
                Labels = new[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" },
                Data = new[] { 120, 180, 200, 250, 220, 300, 270 }
            };

            ViewData["TopMovies"] = new
            {
                Labels = new[] { "Inception", "Avengers", "Jurassic Park", "The Lion King" },
                Data = new[] { 300, 250, 200, 150 }
            };
        }
    }
}