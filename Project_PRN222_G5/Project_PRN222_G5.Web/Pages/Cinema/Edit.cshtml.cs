using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project_PRN222_G5.Domain.Entities.Cinema;
using Project_PRN222_G5.Infrastructure.Data;

namespace Project_PRN222_G5.Web.Pages.Cinema
{
    public class EditModel : PageModel
    {
        private readonly Project_PRN222_G5.Infrastructure.Data.TheDbContext _context;

        public EditModel(Project_PRN222_G5.Infrastructure.Data.TheDbContext context)
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

            var cinema =  await _context.Cinemas.FirstOrDefaultAsync(m => m.Id == id);
            if (cinema == null)
            {
                return NotFound();
            }
            Cinema = cinema;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Cinema).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CinemaExists(Cinema.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CinemaExists(Guid id)
        {
            return _context.Cinemas.Any(e => e.Id == id);
        }
    }
}
