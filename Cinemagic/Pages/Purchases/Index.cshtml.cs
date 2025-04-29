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

        public async Task OnGetAsync(string SearchString)
        {
            // טוען את רכישות, כולל את הקשר למנוי, סרטים וסדרות
            IQueryable<Purchase> PurchaseQuery = from p in _context.Purchases
                                                 .Include(p => p.Members)  // טוען את המנויים
                                                 .Include(p => p.Movies)   // טוען את הסרטים
                                                 .Include(p => p.Series)   // טוען את הסדרות
                                                 select p;

            // חיפוש לפי שם משפחה ושם פרטי
            if (!string.IsNullOrEmpty(SearchString))
            {
                PurchaseQuery = PurchaseQuery.Where(p => p.Members.LastName.Contains(SearchString)
                                                       || p.Members.FirstMidName.Contains(SearchString)
                                                       || p.Movies.MovieName.Contains(SearchString)
                                                       || p.Series.SerieName.Contains(SearchString));
            }

            // שמירת התוצאות
            Purchase = await PurchaseQuery.ToListAsync();
        }
    }
}
