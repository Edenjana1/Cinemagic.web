using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Cinemagic.Data;
using Cinemagic.Models;

namespace Cinemagic.Pages.Purchases
{
    public class IndexModel : PageModel
    {
        private readonly Cinemagic.Data.CinemagicContext _context;

        public IndexModel(Cinemagic.Data.CinemagicContext context)
        {
            _context = context;
        }

        public IList<Purchase> Purchase { get; set; } = default!;

        public bool IsAdmin { get; set; }

        public async Task OnGetAsync(string? SearchString)
        {
            IsAdmin = HttpContext.Session.GetString("UserType") == "Admin";

            // קבלת MemberID כ-string (יכול להיות שאצלה זה string ולא int)
            string? userIdStr = HttpContext.Session.GetString("UserId");

            IQueryable<Purchase> PurchaseQuery = _context.Purchases
                .Include(p => p.Members)
                .Include(p => p.Movies)
                .Include(p => p.Series);

            if (!IsAdmin && !string.IsNullOrEmpty(userIdStr))
            {
                // אם MemberID הוא int, יש להמיר
                if (int.TryParse(userIdStr, out int userId))
                {
                    PurchaseQuery = PurchaseQuery.Where(p => p.Members.MemberID == userId);
                }
                else
                {
                    // טיפול במקרה שה-MemberID לא מספרי (לפי מבנה ה-DB שלך)
                    // למשל: PurchaseQuery = PurchaseQuery.Where(p => p.Members.MemberIdString == userIdStr);
                }
            }

            if (!string.IsNullOrEmpty(SearchString))
            {
                PurchaseQuery = PurchaseQuery.Where(p =>
                    p.Members.LastName.Contains(SearchString) ||
                    p.Members.FirstMidName.Contains(SearchString) ||
                    p.Movies.MovieName.Contains(SearchString) ||
                    p.Series.SerieName.Contains(SearchString)
                );
            }

            Purchase = await PurchaseQuery.ToListAsync();
        }
    }
}
