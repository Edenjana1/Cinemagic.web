using Cinemagic.Data;
using Cinemagic.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

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

        // ������ �-ID �� ������ ������ �����
        public int UserId = 1;

        // ����� ������ URL ������
        public Dictionary<int, string> MovieUrls = new Dictionary<int, string>()
        {
            { 1, "https://hdtodayz.to/movie/watch-wicked-hd-117565" },
            
            // ���� ��� �� �� ������ �-URL �� ������
        };

        // ����� ������ URL ������
        public Dictionary<int, string> SeriesUrls = new Dictionary<int, string>()
        {
            
            // ���� ��� �� �� ������ �-URL �� ������
        };

        public async Task OnGetAsync()
        {
            MoviePurchases = await _context.Purchases
                .Include(p => p.Movies)
                .Where(p => p.MemberID == UserId && p.MovieID != null)
                .ToListAsync();

            SeriesPurchases = await _context.Purchases
                .Include(p => p.Series)
                .Where(p => p.MemberID == UserId && p.SerieID != null)
                .ToListAsync();
        }
    }
}
