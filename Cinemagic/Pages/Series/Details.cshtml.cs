using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Cinemagic.Data;
using Cinemagic.Models;

namespace Cinemagic.Pages.Series
{
    public class DetailsModel : PageModel
    {
        private readonly Cinemagic.Data.CinemagicContext _context;

        public DetailsModel(Cinemagic.Data.CinemagicContext context)
        {
            _context = context;
        }

        public Serie Serie { get; set; } = default!;

        public List<Comment> SerieComments { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serie = await _context.Series.FirstOrDefaultAsync(m => m.SerieID == id);
            if (serie == null)
            {
                return NotFound();
            }
            else
            {
                Serie = serie;
            }

            // טוענים תגובות לסדרה הנבחרת בלבד
            SerieComments = await _context.Comments
                .Where(c => c.SerieID == id)
                .OrderByDescending(c => c.CommentDate)
                .ToListAsync();

            return Page();
        }
        public Comment NewComment { get; set; }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!ModelState.IsValid || id == null)
            {
                return Page();
            }

            NewComment.SerieID = id.Value;
            NewComment.CommentDate = DateTime.Now;

            _context.Comments.Add(NewComment);
            await _context.SaveChangesAsync();

            return RedirectToPage(new { id });
        }
    }
}
