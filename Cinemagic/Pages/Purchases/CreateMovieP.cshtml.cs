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
            Purchase = new Purchase
            {
                MovieID = movie.MovieID,
                PurchaseDate = DateTime.Now,
                Total = movie.MoviePrice
            };

            var memberIdString = HttpContext.Session.GetString("UserId");
            if (!string.IsNullOrEmpty(memberIdString) && int.TryParse(memberIdString, out int memberId))
            {
                var member = await _context.Members.FirstOrDefaultAsync(m => m.MemberID == memberId);
                if (member != null)
                {
                    Purchase.MemberID = member.MemberID;
                    IdentityCard = member.IdintityCard.ToString(); // ודא שזה שם השדה הנכון במודל Member
                }
            }

            ViewData["Email"] = new SelectList(await _context.Members.Select(m => m.Email).ToListAsync());

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

            _context.Purchases.Add(Purchase);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
