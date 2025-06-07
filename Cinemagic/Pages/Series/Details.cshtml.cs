using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Cinemagic.Data;
using Cinemagic.Models;
using Microsoft.AspNetCore.Http; // לשימוש ב-Session

namespace Cinemagic.Pages.Series
{
    public class DetailsModel : PageModel
    {
        private readonly CinemagicContext _context;

        public DetailsModel(CinemagicContext context)
        {
            _context = context;
        }

        public Serie Serie { get; set; } = default!;

        public List<Comment> SerieComments { get; set; } = new();

        [BindProperty]
        public Comment NewComment { get; set; } = new();

        // חדש: האם הסדרה כבר נקנתה
        public bool IsPurchased { get; set; } = false;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Serie = await _context.Series.FirstOrDefaultAsync(m => m.SerieID == id);
            if (Serie == null)
            {
                return NotFound();
            }

            // בדיקה אם המשתמש מחובר וקנה את הסדרה
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (!string.IsNullOrEmpty(userIdStr) && int.TryParse(userIdStr, out int userId))
            {
                IsPurchased = await _context.Purchases
                    .AnyAsync(p => p.MemberID == userId && p.SerieID == Serie.SerieID);
            }

            SerieComments = await _context.Comments
                .Where(c => c.SerieID == id)
                .OrderByDescending(c => c.CommentDate)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Serie = await _context.Series.FirstOrDefaultAsync(m => m.SerieID == id);
            SerieComments = await _context.Comments
                .Where(c => c.SerieID == id)
                .OrderByDescending(c => c.CommentDate)
                .ToListAsync();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            NewComment.SerieID = id.Value;
            NewComment.CommentDate = DateTime.Now;

            _context.Comments.Add(NewComment);
            await _context.SaveChangesAsync();

            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostDeleteCommentAsync(int commentId, int id)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage(new { id });
        }
    }
}
