using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Project_PRN222_G5.Web.Controllers
{
    [AllowAnonymous]
    public class PagesController : Controller
    {
        [HttpGet("")] public IActionResult Home() => View();

        [HttpGet]
        public IActionResult Privacy() => View();

        [HttpGet] public IActionResult About() => View();
    }
}