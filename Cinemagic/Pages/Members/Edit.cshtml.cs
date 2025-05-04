using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cinemagic.Data;
using Cinemagic.Models;

namespace Cinemagic.Pages.Members
{
    public class EditModel : PageModel
    {
        private readonly Cinemagic.Data.CinemagicContext _context;

        public EditModel(Cinemagic.Data.CinemagicContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Member Member { get; set; } = default!;

        public List<SelectListItem> GenderOptions { get; set; } = new List<SelectListItem>();

        public List<SelectListItem> ImageOptions { get; set; } = new List<SelectListItem>();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member =  await _context.Members.FirstOrDefaultAsync(m => m.MemberID == id);
            if (member == null)
            {
                return NotFound();
            }
            Member = member;

            ImageOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Girl", Value = "girl_icon.jpg" },
                new SelectListItem { Text = "Boy", Value = "boy_icon.jpg" },
                new SelectListItem { Text = "Woman", Value = "woman_icon.jpg" },
                new SelectListItem { Text = "Man", Value = "man_icon.jpg" }
            };

            GenderOptions = Enum.GetValues(typeof(Gender))
                .Cast<Gender>()
                .Select(g => new SelectListItem
                {
                    Text = g.ToString(),  // הערך של המגדר (Male, Female)
                    Value = g.ToString()   // שמירת הערך (Male, Female)
                }).ToList();

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Member).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(Member.MemberID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.MemberID == id);
        }
    }
}
