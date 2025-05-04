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

        public IList<Serie> Serie { get;set; } = default!;
        

        [BindProperty(SupportsGet = true)]
        public List<SelectListItem> Genres { get; set; } = new();
        [BindProperty(SupportsGet = true)]
        public SerieGenre? Genre { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public async Task OnGetAsync()
        {
            // יצירת רשימת ז'אנרים מתוך ה-enum
            Genres = Enum.GetValues(typeof(MovieGenre))
                .Cast<SerieGenre>()
                .Select(g => new SelectListItem
                {
                    Value = g.ToString(),
                    Text = g.ToString() // אפשר לשנות כאן לעברית אם רוצים
                })
                .ToList();

            var query = _context.Series.AsQueryable();

            if (Genre.HasValue)
            {
                query = query.Where(m => m.SerieGenre == Genre.Value);
            }

            if (!string.IsNullOrEmpty(SearchString))
            {
                query = query.Where(m => m.SerieName.Contains(SearchString));
            }

            Serie = await query.ToListAsync();
        }
    }
}