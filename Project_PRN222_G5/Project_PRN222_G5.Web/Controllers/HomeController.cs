using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Project_PRN222_G5.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public IActionResult Home()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Privacy() => View();
    }
}