using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project_PRN222_G5.DataAccess.Data;
using Project_PRN222_G5.DataAccess.Entities.Movies;

namespace Project_PRN222_G5.Web.Pages.Cinema
{
    public class DetailsModel : PageModel
    {
        private readonly TheDbContext _context;

        public DetailsModel(TheDbContext context)
        {
            _context = context;
        }

        public Movie Movie { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            else
            {
                Movie = movie;
            }
            return Page();
        }
    }
}