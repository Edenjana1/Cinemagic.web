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
        public IActionResult OnGet()
        {
            ImageOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "DUNE", Value = "DUNE.jpg" },
                new SelectListItem { Text = "WICKED", Value = "WICKED.jpg" },
                new SelectListItem { Text = "Oppenhaimer", Value = "oppenheimer.jpg" },
                new SelectListItem { Text = "Harry potter and the philosopher's stone", Value = "harry_potter1.jpg" }
            };
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
