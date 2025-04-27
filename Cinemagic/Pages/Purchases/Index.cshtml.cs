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

        public IList<Purchase> Purchase { get;set; } = default!;

        public async Task OnGetAsync(string SearchString)
        {
            
            IQueryable<Purchase> PurchaseID = from s in _context.Purchases select s;

            if (!string.IsNullOrEmpty(SearchString))
            {
                PurchaseID = PurchaseID.Where(s => s.Members.LastName.Contains(SearchString) || s.Members.FirstMidName.Contains(SearchString));
            }

            Purchase = await PurchaseID.ToListAsync();
            //Purchase = await _context.Purchases.ToListAsync();
            
                
        }
    }
}
