using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project_PRN222_G5.Domain.Entities.Movie;

namespace Project_PRN222_G5.Web.Pages.Cinema
{
    public class IndexModel : PageModel
    {
        private readonly Project_PRN222_G5.Infrastructure.Data.TheDbContext _context;

        public IndexModel(Project_PRN222_G5.Infrastructure.Data.TheDbContext context)
        {
            _context = context;
        }

        public IList<Movie> Movie { get; set; } = null!;

        public async Task OnGetAsync()
        {
            Movie = await _context.Movies.ToListAsync();
        }
    }
}
