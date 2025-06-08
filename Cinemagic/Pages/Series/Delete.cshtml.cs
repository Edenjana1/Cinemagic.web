using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Cinemagic.Data;
using Cinemagic.Models;

namespace Cinemagic.Pages.Series
{
    public class DeleteModel : PageModel
    {
        private readonly Cinemagic.Data.CinemagicContext _context;

        public DeleteModel(Cinemagic.Data.CinemagicContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Serie Serie { get; set; } = default!;


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Serie = await _context.Series.FirstOrDefaultAsync(m => m.SerieID == id);
            if (Serie == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Serie == null || Serie.SerieID == 0)
            {
                return NotFound();
            }

            var relatedComments = await _context.Comments
    .Where(c => c.SerieID == Serie.SerieID)
    .ToListAsync();

            _context.Comments.RemoveRange(relatedComments);


            var serieToDelete = await _context.Series.FindAsync(Serie.SerieID);
            if (serieToDelete != null)
            {
                _context.Series.Remove(serieToDelete);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

    }
}
