using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Cinemagic.Data;
using Cinemagic.Models;

namespace Cinemagic.Pages.Purchases
{
    public class CreateModel : PageModel
    {
        private readonly Cinemagic.Data.CinemagicContext _context;

        public CreateModel(Cinemagic.Data.CinemagicContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            // יצירת רשימות אפשרויות עבור רכישה
            ViewData["MovieID"] = new SelectList(_context.Movies, "MovieID", "MovieName");
            ViewData["SerieID"] = new SelectList(_context.Series, "SerieID", "SerieName");
            ViewData["MemberID"] = new SelectList(_context.Members, "MemberID", "IdintityCard");

            // יצירת רשימת אימיילים מתוך המנויים
            ViewData["Email"] = new SelectList(_context.Members, "Email", "Email");

            return Page();
        }

        [BindProperty]
        public Purchase Purchase { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // הוספת רכישה למסד הנתונים
            _context.Purchases.Add(Purchase);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}