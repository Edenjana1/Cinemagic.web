using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Cinemagic.Data;
using Cinemagic.Models;

namespace Cinemagic.Pages.Movies
{
    public class CreateModel : PageModel
    {
        private readonly Cinemagic.Data.CinemagicContext _context;

        public CreateModel(Cinemagic.Data.CinemagicContext context)
        {
            _context = context;
        }

        public List<SelectListItem> ImageOptions { get; set; } = new List<SelectListItem>();

        public List<SelectListItem> Genres { get; set; }
        public IActionResult OnGet()
        {
            ImageOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "DUNE", Value = "DUNE.jpg" },
                new SelectListItem { Text = "WICKED", Value = "WICKED.jpg" },
                new SelectListItem { Text = "Oppenhaimer", Value = "oppenheimer.jpg" },
                new SelectListItem { Text = "Harry potter and the philosopher's stone", Value = "harry_potter1.jpg" },
                new SelectListItem { Text = "Counterstrike", Value = "Counterstrike.jpg" },
                new SelectListItem { Text = "Fear Street: Prom Queen", Value = "FearStreetPromQueen.jpg" },
                new SelectListItem { Text = "Plankton: The Movie", Value = "PlanktonTheMovie.jpg" },
                new SelectListItem { Text = "Revelations", Value = "Revelations.jpg" },
                new SelectListItem { Text = "The Electric State", Value = "TheElectricState.jpg" }
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
