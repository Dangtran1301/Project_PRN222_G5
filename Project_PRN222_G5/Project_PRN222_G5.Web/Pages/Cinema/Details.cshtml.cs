using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project_PRN222_G5.Domain.Entities.Cinema;
using Project_PRN222_G5.Infrastructure.Data;

namespace Project_PRN222_G5.Web.Pages.Cinema
{
    public class DetailsModel : PageModel
    {
        private readonly Project_PRN222_G5.Infrastructure.Data.TheDbContext _context;

        public DetailsModel(Project_PRN222_G5.Infrastructure.Data.TheDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Project_PRN222_G5.Domain.Entities.Cinema.Cinema Cinema { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cinema = await _context.Cinemas.FirstOrDefaultAsync(m => m.Id == id);
            if (cinema == null)
            {
                return NotFound();
            }
            else
            {
                Cinema = cinema;
            }
            return Page();
        }
    }
}
