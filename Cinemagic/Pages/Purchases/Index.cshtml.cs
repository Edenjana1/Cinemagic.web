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

            // שליפת ת"ז מה-Session והמרה ל-int
            string? identityCardStr = HttpContext.Session.GetString("IdentityCard");
            int? identityCard = null;

            if (int.TryParse(identityCardStr, out int parsedId))
            {
                identityCard = parsedId;
            }

            IQueryable<Purchase> PurchaseQuery = _context.Purchases
                .Include(p => p.Members)
                .Include(p => p.Movies)
                .Include(p => p.Series);

            // אם המשתמש הוא מנוי, הצג רק את הרכישות שלו
            if (!IsAdmin && identityCard != null)
            {
                PurchaseQuery = PurchaseQuery.Where(p => p.Members.IdintityCard == identityCard);
            }

            // סינון לפי טקסט חיפוש
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
