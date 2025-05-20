using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CinemaEntity = Project_PRN222_G5.Domain.Entities.Cinema.Cinema;
using Project_PRN222_G5.Infrastructure.Data;

namespace Project_PRN222_G5.Web.Pages.Cinema
{
    public class IndexModel : PageModel
    {
        private readonly Project_PRN222_G5.Infrastructure.Data.TheDbContext _context;

        public IndexModel(Project_PRN222_G5.Infrastructure.Data.TheDbContext context)
        {
            _context = context;
        }
        public IList<CinemaEntity> Cinema { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Cinema = await _context.Cinemas.ToListAsync();
        }
    }
}
