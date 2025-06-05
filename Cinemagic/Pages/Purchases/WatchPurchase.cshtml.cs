using Cinemagic.Data;
using Cinemagic.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using static System.Net.WebRequestMethods;

namespace Cinemagic.Pages.Purchases
{
    public class WatchPurchaseModel : PageModel
    {
        private readonly CinemagicContext _context;

        public WatchPurchaseModel(CinemagicContext context)
        {
            _context = context;
        }

        public List<Purchase> MoviePurchases { get; set; } = new();
        public List<Purchase> SeriesPurchases { get; set; } = new();

        // להחליף ל-ID של המשתמש המחובר בפועל
        public int UserId = 1;

        // מילון כתובות URL לסרטים
        public Dictionary<int, string> MovieUrls = new Dictionary<int, string>()
        {
            { 1, "https://www6.f2movies.to/movie/wicked-117565" },
            { 2, "https://www6.f2movies.to/movie/dune-6752" },
            
            // הוסף כאן את כל כתובות ה-URL של הסרטים
        };

        // מילון כתובות URL לסדרות
        public Dictionary<int, string> SeriesUrls = new Dictionary<int, string>()
        {
            { 1, "https://www6.f2movies.to/tv/game-of-thrones-39539" },
            { 2, "https://www6.f2movies.to/tv/cobra-kai-38555" },
            // הוסף כאן את כל כתובות ה-URL של הסדרות
        };

        public async Task OnGetAsync()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
            {
                // אפשר להפנות לדף התחברות או להציג הודעה שאין משתמש
                MoviePurchases = new List<Purchase>();
                SeriesPurchases = new List<Purchase>();
                return;
            }

            // שליפת רכישות לפי המשתמש שמחובר בפועל
            MoviePurchases = await _context.Purchases
                .Include(p => p.Movies)
                .Where(p => p.MemberID == userId && p.MovieID != null)
                .ToListAsync();

            SeriesPurchases = await _context.Purchases
                .Include(p => p.Series)
                .Where(p => p.MemberID == userId && p.SerieID != null)
                .ToListAsync();
        }

    }
}
