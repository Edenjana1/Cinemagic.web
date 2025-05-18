using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Cinemagic.Data;
using Cinemagic.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;

namespace Cinemagic.Pages.Series
{
    public class IndexModel : PageModel
    {
        private readonly Cinemagic.Data.CinemagicContext _context;

        public IndexModel(Cinemagic.Data.CinemagicContext context)
        {
            _context = context;
        }

        public IList<Serie> Serie { get; set; } = default!;


        [BindProperty(SupportsGet = true)]
        public List<SelectListItem> Genres { get; set; } = new();
        [BindProperty(SupportsGet = true)]
        public SerieGenre? Genre { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public List<Serie> MostPurchasedSeries { get; set; } = new();

        public async Task OnGetAsync()
        {
            // יצירת רשימת ז'אנרים מתוך enum
            Genres = Enum.GetValues(typeof(SerieGenre)) // שימי לב שזו הייתה טעות – MovieGenre => SerieGenre
                .Cast<SerieGenre>()
                .Select(g => new SelectListItem
                {
                    Value = g.ToString(),
                    Text = g.ToString()
                })
                .ToList();

            var query = _context.Series.AsQueryable();

            if (Genre.HasValue)
            {
                query = query.Where(s => s.SerieGenre == Genre.Value);
            }

            if (!string.IsNullOrEmpty(SearchString))
            {
                query = query.Where(s => s.SerieName.Contains(SearchString));
            }

            Serie = await query.ToListAsync();

            // --- רשימת הסדרות הכי נרכשות ---
            MostPurchasedSeries = await _context.Purchases
                .Where(p => p.SerieID != null)
                .GroupBy(p => p.SerieID)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .Take(5)
                .Join(_context.Series,
                    serieId => serieId,
                    serie => serie.SerieID,
                    (serieId, serie) => serie)
                .ToListAsync();
        }
    }
}