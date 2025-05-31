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
    public class CreateMoviePModel : PageModel
    {
        private readonly CinemagicContext _context;

        public CreateMoviePModel(CinemagicContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Purchase Purchase { get; set; } = default!;

        public string? MovieName { get; set; }

        public IActionResult OnGet(int? movieid)
        {
            if (movieid.HasValue)
            {
                var movie = _context.Movies.FirstOrDefault(m => m.MovieID == movieid.Value);
                if (movie != null)
                {
                    MovieName = movie.MovieName; // כאן שומרים את שם הסרט להצגה
                    Purchase = new Purchase
                    {
                        MovieID = movie.MovieID,
                        PurchaseDate = DateTime.Now
                    };
                }
            }
            else
            {
                Purchase = new Purchase
                {
                    PurchaseDate = DateTime.Now
                };
            }

            // הצגת שם הסרט + המחיר לבחירה ב-Dropdown
            ViewData["MovieID"] = new SelectList(
                _context.Movies.Select(m => new
                {
                    m.MovieID,
                    Display = m.MovieName + " (" + m.MoviePrice + "₪)"
                }),
                "MovieID",
                "Display"
            );

            //// הצגת שם הסדרה + המחיר לבחירה ב-Dropdown
            //ViewData["SerieID"] = new SelectList(
            //    _context.Series.Select(s => new
            //    {
            //        s.SerieID,
            //        Display = s.SerieName + " (" + s.SeriePrice + "₪)"
            //    }),
            //    "SerieID",
            //    "Display"
            //);

            ViewData["MemberID"] = new SelectList(_context.Members, "MemberID", "IdintityCard");
            ViewData["Email"] = new SelectList(_context.Members, "Email", "Email");

            // שליחת מחירי סרטים וסדרות ל-JavaScript
            var moviePrices = _context.Movies.ToDictionary(m => m.MovieID.ToString(), m => m.MoviePrice);
            //var seriePrices = _context.Series.ToDictionary(s => s.SerieID.ToString(), s => s.SeriePrice);

            ViewData["MoviePrices"] = JsonSerializer.Serialize(moviePrices);
            //ViewData["SeriePrices"] = JsonSerializer.Serialize(seriePrices);

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // אם לא נבחר תאריך, קובעים תאריך ברירת מחדל להיום
            if (Purchase.PurchaseDate == default)
            {
                Purchase.PurchaseDate = DateTime.Now;
            }

            decimal total = 0;

            if (Purchase.MovieID.HasValue)
            {
                var movie = await _context.Movies.FindAsync(Purchase.MovieID.Value);
                if (movie != null)
                {
                    total += movie.MoviePrice;
                }
            }

            if (Purchase.SerieID.HasValue)
            {
                var serie = await _context.Series.FindAsync(Purchase.SerieID.Value);
                if (serie != null)
                {
                    total += serie.SeriePrice;
                }
            }

            Purchase.Total = total;

            _context.Purchases.Add(Purchase);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}