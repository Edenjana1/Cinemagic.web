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
        public int IdentityCard { get; set; }

        public IActionResult OnGet(int? movieid)
        {
            // קבלת ת"ז מה-Session
            int? identityCard = HttpContext.Session.GetInt32("IdentityCard");
            if (identityCard == null)
            {
                return RedirectToPage("/Login"); // אם אין ת"ז – החזר לכניסה
            }

            // חיפוש המנוי במסד הנתונים לפי ת"ז
            var member = _context.Members.FirstOrDefault(m => m.IdintityCard == identityCard.Value);
            if (member == null)
            {
                return NotFound("Member not found.");
            }

            // הגדרת הנתונים ההתחלתיים לרכישה
            Purchase = new Purchase
            {
                MemberID = member.MemberID,
                PurchaseDate = DateTime.Now
            };

            IdentityCard = member.IdintityCard;

            if (movieid.HasValue)
            {
                var movie = _context.Movies.FirstOrDefault(m => m.MovieID == movieid.Value);
                if (movie != null)
                {
                    MovieName = movie.MovieName;
                    Purchase.MovieID = movie.MovieID;
                }
            }

            // רשימות לבחירת אימייל
            ViewData["Email"] = new SelectList(_context.Members, "Email", "Email");

            // שליחת מחירי סרטים ל-JavaScript
            var moviePrices = _context.Movies.ToDictionary(m => m.MovieID.ToString(), m => m.MoviePrice);
            ViewData["MoviePrices"] = JsonSerializer.Serialize(moviePrices);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            // ודא שהת"ז קיימת ב-Session
            int? identityCard = HttpContext.Session.GetInt32("IdentityCard");
            if (identityCard == null)
                return RedirectToPage("/Login");

            var member = _context.Members.FirstOrDefault(m => m.IdintityCard == identityCard.Value);
            if (member == null)
                return NotFound("Member not found");

            Purchase.MemberID = member.MemberID;

            if (Purchase.PurchaseDate == default)
                Purchase.PurchaseDate = DateTime.Now;

            // חישוב מחיר כולל
            decimal total = 0;

            if (Purchase.MovieID.HasValue)
            {
                var movie = await _context.Movies.FindAsync(Purchase.MovieID.Value);
                if (movie != null)
                    total += movie.MoviePrice;
            }

            if (Purchase.SerieID.HasValue)
            {
                var serie = await _context.Series.FindAsync(Purchase.SerieID.Value);
                if (serie != null)
                    total += serie.SeriePrice;
            }

            Purchase.Total = total;

            _context.Purchases.Add(Purchase);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}