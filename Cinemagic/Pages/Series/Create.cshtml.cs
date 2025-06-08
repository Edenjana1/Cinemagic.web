using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Cinemagic.Data;
using Cinemagic.Models;
using Humanizer.Localisation;

namespace Cinemagic.Pages.Series
{
    public class CreateModel : PageModel
    {
        private readonly Cinemagic.Data.CinemagicContext _context;

        public CreateModel(Cinemagic.Data.CinemagicContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Serie Serie { get; set; } = default!;

        public List<SelectListItem> ImageOptions { get; set; } = new List<SelectListItem>();

        public List<SelectListItem> Genres { get; set; }

        public IActionResult OnGet()
        {
            ImageOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Cobra Kai", Value = "CobraKai.jpg" },
                new SelectListItem { Text = "Game Of Thrones", Value = "GameOfThrones.jpg" },
                new SelectListItem { Text = "Money Heist", Value = "MoneyHeist.jpg" },
                new SelectListItem { Text = "Stranger Things", Value = "StrangerThings.jpg" },
                new SelectListItem { Text = "Friends", Value = "Friends.jpg" },
                new SelectListItem { Text = "Adolescence ", Value = "Adolescence.jpg" },
                new SelectListItem { Text = "The Residance", Value = "TheResidance.jpg" }
            };

            Genres = Enum.GetValues(typeof(SerieGenre))
                 .Cast<SerieGenre>()
                 .Select(g => new SelectListItem
                 {
                     Value = g.ToString(),
                     Text = g.ToString()
                 })
                 .ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Series.Add(Serie);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

