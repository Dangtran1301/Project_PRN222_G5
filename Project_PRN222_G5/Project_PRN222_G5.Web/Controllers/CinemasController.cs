using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Cinema;
using Project_PRN222_G5.DataAccess.DTOs;

namespace Project_PRN222_G5.Web.Controllers
{
    [AllowAnonymous]
    public class CinemasController(ICinemaService cinemaService) : Controller
    {
        private readonly ICinemaService _cinemaService = cinemaService;

        // GET: Cinemas
        public async Task<IActionResult> Index(string? search, CancellationToken cancellationToken)
        {
            var request = new PagedRequest
            {
                PageNumber = 1,
                PageSize = 100,
                Search = search
            };

            var result = await _cinemaService.GetPagedAsync(request, cancellationToken: cancellationToken);
            ViewBag.Search = search;
            return View(result.Data);
        }

        // GET: Cinemas/Details/5
        public async Task<IActionResult> Details(Guid? id, CancellationToken cancellationToken)
        {
            if (id == null) return NotFound();

            var cinema = await _cinemaService.GetByIdAsync(id.Value, cancellationToken);

            return View(cinema);
        }
    }
}