using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Cinemagic.Data;
using Cinemagic.Models;

namespace Cinemagic.Pages.Movies
{
    public class DeleteModel : PageModel
    {
        private readonly Cinemagic.Data.CinemagicContext _context;

        public DeleteModel(Cinemagic.Data.CinemagicContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Movie Movie { get; set; } = default!;


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Movie = await _context.Movies.FirstOrDefaultAsync(m => m.MovieID == id);
            if (Movie == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Movie == null || Movie.MovieID == 0)
            {
                return NotFound();
            }

            var relatedComments = await _context.Comments
    .Where(c => c.MovieID == Movie.MovieID)
    .ToListAsync();

            _context.Comments.RemoveRange(relatedComments);


            var MovieToDelete = await _context.Movies.FindAsync(Movie.MovieID);
            if (MovieToDelete != null)
            {
                _context.Movies.Remove(MovieToDelete);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

    }
}
