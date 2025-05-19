using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Cinemagic.Data;
using Cinemagic.Models;
using System.Text.Json;

namespace Cinemagic.Pages.Purchases
{
    public class CreateSeriePModel : PageModel
    {
        private readonly CinemagicContext _context;

        public CreateSeriePModel(CinemagicContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Purchase Purchase { get; set; } = default!;

        public string? SerieName { get; set; }

        public IActionResult OnGet(int? serieid)
        {
            if (serieid.HasValue)
            {
                var serie = _context.Series.FirstOrDefault(m => m.SerieID == serieid.Value);
                if (serie != null)
                {
                    SerieName = serie.SerieName; // שם הסדרה להצגה בלבד
                    Purchase = new Purchase
                    {
                        SerieID = serie.SerieID,
                        PurchaseDate = DateTime.Now
                    };
                }
            }
            else
            {
                if (Purchase == null || Purchase.PurchaseDate == default)
                {
                    Purchase = new Purchase
                    {
                        PurchaseDate = DateTime.Now
                    };
                }
            }

            // --- אפשר להסיר את רשימת הסדרות ---
            // ViewData["SerieID"] = new SelectList(...);

            ViewData["MovieID"] = new SelectList(
                _context.Movies.Select(m => new
                {
                    m.MovieID,
                    Display = m.MovieName + " (" + m.MoviePrice + "₪)"
                }),
                "MovieID",
                "Display"
            );

            ViewData["MemberID"] = new SelectList(_context.Members, "MemberID", "IdintityCard");
            ViewData["Email"] = new SelectList(_context.Members, "Email", "Email");

            var moviePrices = _context.Movies.ToDictionary(m => m.MovieID.ToString(), m => m.MoviePrice);
            var seriePrices = _context.Series.ToDictionary(s => s.SerieID.ToString(), s => s.SeriePrice);

            ViewData["MoviePrices"] = JsonSerializer.Serialize(moviePrices);
            ViewData["SeriePrices"] = JsonSerializer.Serialize(seriePrices);

            return Page();
        }



        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // אם לא נבחר תאריך, קובעים את התאריך לעכשיו
            if (Purchase.PurchaseDate == default)
            {
                Purchase.PurchaseDate = DateTime.Now;
            }

            decimal total = 0;

            // בדיקה אם נבחר סרט (MovieID לא ריק)
            if (Purchase.MovieID.HasValue)
            {
                var movie = await _context.Movies.FindAsync(Purchase.MovieID.Value);
                if (movie != null)
                {
                    total += movie.MoviePrice;
                }
            }

            // בדיקה אם נבחרה סדרה (SerieID לא ריק)
            if (Purchase.SerieID.HasValue)
            {
                var serie = await _context.Series.FindAsync(Purchase.SerieID.Value);
                if (serie != null)
                {
                    total += serie.SeriePrice;
                }
            }

            // עדכון הסכום הסופי
            Purchase.Total = total;

            // שמירה למסד הנתונים
            _context.Purchases.Add(Purchase);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}