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

namespace Cinemagic.Pages.Series
{
    public class EditModel : PageModel
    {
        private readonly Cinemagic.Data.CinemagicContext _context;

        public EditModel(Cinemagic.Data.CinemagicContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Serie Serie { get; set; } = default!;

        public List<SelectListItem> ImageOptions { get; set; } = new List<SelectListItem>();

        public List<SelectListItem> Genres { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serie =  await _context.Series.FirstOrDefaultAsync(m => m.SerieID == id);
            if (serie == null)
            {
                return NotFound();
            }
            Serie = serie;

            ImageOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Cobra Kai", Value = "CobraKai.jpg" },
                new SelectListItem { Text = "Game Of Thrones", Value = "GameOfThrones.jpg" },
                new SelectListItem { Text = "Money Heist", Value = "MoneyHeist.jpg" },
                new SelectListItem { Text = "Stranger Things", Value = "StrangerThings.jpg" },
                new SelectListItem { Text = "Friends", Value = "Friends.jpg" }
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Serie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SerieExists(Serie.SerieID))
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

        private bool SerieExists(int id)
        {
            return _context.Series.Any(e => e.SerieID == id);
        }

    }
}
