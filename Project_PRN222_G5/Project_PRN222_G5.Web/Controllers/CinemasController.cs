using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Cinema;
using Project_PRN222_G5.DataAccess.DTOs;

namespace Project_PRN222_G5.Web.Controllers
{
    [AllowAnonymous]
    public class CinemasController : Controller
    {
        private readonly ICinemaService _cinemaService;

        public CinemasController(ICinemaService cinemaService)
        {
            _cinemaService = cinemaService;
        }

        // GET: Cinemas
        public async Task<IActionResult> Index(string? search)
        {
            var request = new PagedRequest
            {
                PageNumber = 1,
                PageSize = 100,
                Search = search
            };

            var result = await _cinemaService.GetPagedAsync(request);
            ViewBag.Search = search;
            return View(result.Data);
        }

        // GET: Cinemas/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var cinema = await _cinemaService.GetByIdAsync(id.Value);
            if (cinema == null) return NotFound();

            return View(cinema);
        }
    }
}