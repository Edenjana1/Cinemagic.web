using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cinemagic.Data;
using Cinemagic.Models;

namespace Cinemagic.Pages.Movies
{
    public class EditModel : PageModel
    {
        private readonly Cinemagic.Data.CinemagicContext _context;

        public EditModel(Cinemagic.Data.CinemagicContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Movie Movie { get; set; } = default!;
        public List<SelectListItem> ImageOptions { get; set; } = new List<SelectListItem>();

        public List<SelectListItem> Genres { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie =  await _context.Movies.FirstOrDefaultAsync(m => m.MovieID == id);
            if (movie == null)
            {
                return NotFound();
            }
            Movie = movie;

            ImageOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "DUNE", Value = "DUNE.jpg" },
                new SelectListItem { Text = "WICKED", Value = "WICKED.jpg" },
                new SelectListItem { Text = "Oppenhaimer", Value = "oppenheimer.jpg" },
                new SelectListItem { Text = "Harry potter and the philosopher's stone", Value = "harry_potter1.jpg" }
            };
            Genres = Enum.GetValues(typeof(MovieGenre))
                 .Cast<MovieGenre>()
                 .Select(g => new SelectListItem
                 {
                     Value = g.ToString(),
                     Text = g.ToString()
                 })
                 .ToList();
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

            _context.Attach(Movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(Movie.MovieID))
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

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.MovieID == id);
        }
    }
}
