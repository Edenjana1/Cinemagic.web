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
using Microsoft.EntityFrameworkCore;
using Cinemagic.Pages.Cupons;

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

        [BindProperty]
        public string? CouponCode { get; set; }  // <-- הוספתי פה

        public string? SerieName { get; set; }
        public string? IdentityCard { get; set; }

        public async Task<IActionResult> OnGetAsync(int? serieid)
        {
            if (serieid == null)
                return NotFound();

            var serie = await _context.Series.FirstOrDefaultAsync(s => s.SerieID == serieid);
            if (serie == null)
                return NotFound();

            SerieName = serie.SerieName;

            var memberIdString = HttpContext.Session.GetString("UserId");
            if (!string.IsNullOrEmpty(memberIdString) && int.TryParse(memberIdString, out int memberId))
            {
                var member = await _context.Members.FirstOrDefaultAsync(m => m.MemberID == memberId);
                if (member != null)
                {
                    Purchase = new Purchase
                    {
                        SerieID = serie.SerieID,
                        PurchaseDate = DateTime.Now,
                        Total = serie.SeriePrice,
                        MemberID = member.MemberID,
                        Email = member.Email
                    };

                    IdentityCard = member.IdintityCard.ToString();
                }
            }
            else
            {
                Purchase = new Purchase
                {
                    SerieID = serie.SerieID,
                    PurchaseDate = DateTime.Now,
                    Total = serie.SeriePrice
                };
            }

            ViewData["MemberID"] = new SelectList(_context.Members, "MemberID", "IdintityCard");
            ViewData["Email"] = new SelectList(_context.Members, "Email", "Email");

            var moviePrices = await _context.Movies.ToDictionaryAsync(m => m.MovieID.ToString(), m => m.MoviePrice);
            var seriePrices = await _context.Series.ToDictionaryAsync(s => s.SerieID.ToString(), s => s.SeriePrice);

            ViewData["MoviePrices"] = JsonSerializer.Serialize(moviePrices);
            ViewData["SeriePrices"] = JsonSerializer.Serialize(seriePrices);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            if (Purchase.PurchaseDate == default)
                Purchase.PurchaseDate = DateTime.Now;

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

            // בדיקת קוד קופון שמתחיל ב-SERIE (לא תלוי רישיות)
            decimal discount = 0;
            if (!string.IsNullOrEmpty(CouponCode))
            {
                string normalizedCode = CouponCode.Trim().ToUpper();

                if (!normalizedCode.StartsWith("SERIE"))
                {
                    ModelState.AddModelError("CouponCode", "רק קופונים שמתחילים ב-SERIE מותרים לסדרות");
                    return Page();
                }

                var coupon = CouponStore.Coupons
                    .FirstOrDefault(c => c.Code.Trim().ToUpper() == normalizedCode);

                if (coupon == null)
                {
                    ModelState.AddModelError("CouponCode", "קוד קופון לא נמצא");
                    return Page();
                }

                if (coupon.ExpiryDate < DateTime.Today)
                {
                    ModelState.AddModelError("CouponCode", "תוקף הקופון פג");
                    return Page();
                }

                discount = coupon.Discount / 100m;
            }

            Purchase.Total = total * (1 - discount);

            _context.Purchases.Add(Purchase);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }
}
