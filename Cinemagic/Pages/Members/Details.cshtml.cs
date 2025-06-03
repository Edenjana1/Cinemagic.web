using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Cinemagic.Data;
using Cinemagic.Models;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Cinemagic.Pages.Members
{
    public class DetailsModel : PageModel
    {
        private readonly Cinemagic.Data.CinemagicContext _context;

        public DetailsModel(Cinemagic.Data.CinemagicContext context)
        {
            _context = context;
        }

        public Member Member { get; set; } = default!;

        public List<SelectListItem> ImageOptions { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Text = "Girl", Value = "girl_icon.jpg" },
            new SelectListItem { Text = "Boy", Value = "boy_icon.jpg" },
            new SelectListItem { Text = "Woman", Value = "woman_icon.jpg" },
            new SelectListItem { Text = "Man", Value = "man_icon.jpg" }
        };

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            // בדיקת סשן – רק משתמש מחובר יכול לראות את הדף
            var userType = HttpContext.Session.GetString("UserType");
            if (userType != "Member")
            {
                return RedirectToPage("/Login/Index");
            }

            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members.FirstOrDefaultAsync(m => m.MemberID == id);
            if (member == null)
            {
                return NotFound();
            }

            Member = member;
            return Page();
        }
    }
}

