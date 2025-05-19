using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project_PRN222_G5.Domain.Entities.Movie;
using Project_PRN222_G5.Infrastructure.Data;

namespace Project_PRN222_G5.Web.Pages.Cinema
{
    public class CreateModel : PageModel
    {
        private readonly Project_PRN222_G5.Infrastructure.Data.TheDbContext _context;

        public CreateModel(Project_PRN222_G5.Infrastructure.Data.TheDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Movie Movie { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Movies.Add(Movie);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
