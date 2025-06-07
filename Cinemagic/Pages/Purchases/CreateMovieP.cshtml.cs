using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Cinemagic.Data;
using Cinemagic.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        [BindProperty]
        public string? CouponCode { get; set; }

        public string MovieName { get; set; } = "";
        public string IdentityCard { get; set; } = "";

        public async Task<IActionResult> OnGetAsync(int? movieId)
        {
            if (movieId == null)
                return NotFound();

            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.MovieID == movieId);
            if (movie == null)
                return NotFound();

            MovieName = movie.MovieName;

            // טען פרטי משתמש אם קיים
            var memberIdString = HttpContext.Session.GetString("UserId");
            if (!string.IsNullOrEmpty(memberIdString) && int.TryParse(memberIdString, out int memberId))
            {
                var member = await _context.Members.FirstOrDefaultAsync(m => m.MemberID == memberId);
                if (member != null)
                {
                    Purchase = new Purchase
                    {
                        MovieID = movie.MovieID,
                        MemberID = member.MemberID,
                        PurchaseDate = DateTime.Now,
                        Total = movie.MoviePrice
                    };
                    IdentityCard = member.IdintityCard.ToString();
                }
            }
            else
            {
                Purchase = new Purchase
                {
                    MovieID = movie.MovieID,
                    PurchaseDate = DateTime.Now,
                    Total = movie.MoviePrice
                };
            }

            ViewData["Email"] = new SelectList(await _context.Members.Select(m => m.Email).ToListAsync());

            // אין צורך לטעון מחירים כאן אלא אם מתכננים שימוש בג'אווהסקריפט בדף
            // ניתן להשאיר אותם או להסיר אותם בהתאם לצורך
            var moviePrices = await _context.Movies
                .ToDictionaryAsync(m => m.MovieID.ToString(), m => m.MoviePrice);

            var seriePrices = await _context.Series
                .ToDictionaryAsync(s => s.SerieID.ToString(), s => s.SeriePrice);

            ViewData["MoviePrices"] = JsonSerializer.Serialize(moviePrices);
            ViewData["SeriePrices"] = JsonSerializer.Serialize(seriePrices);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            // קבל מחיר הסרט
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.MovieID == Purchase.MovieID);
            if (movie == null)
            {
                ModelState.AddModelError("", "הסרט לא נמצא");
                return Page();
            }

            decimal price = movie.MoviePrice;

            // בדוק את קוד הקופון שהוזן
            decimal discount = 0m;
            if (!string.IsNullOrEmpty(CouponCode))
            {
                // לדוגמה, קוד קופון "MOVIE10" נותן 10% הנחה
                if (CouponCode.Trim().ToUpper() == "MOVIE10")
                {
                    discount = 0.10m;
                }
                else
                {
                    ModelState.AddModelError("CouponCode", "קוד קופון לא תקין");
                    return Page();
                }
            }

            // חשב מחיר סופי עם הנחה
            Purchase.Total = price * (1 - discount);
            Purchase.PurchaseDate = DateTime.Now; // עדכן תאריך רכישה

            _context.Purchases.Add(Purchase);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
