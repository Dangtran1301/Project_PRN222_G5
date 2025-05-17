using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Project_PRN222_G5.Web.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            RedirectToPage("Auth/SignIn");
        }
    }
}