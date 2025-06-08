using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Cinemagic.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace Cinemagic.Pages.Movies
{
    public class DetailsModel : PageModel
    {
        private readonly Cinemagic.Data.CinemagicContext _context;

        public DetailsModel(Cinemagic.Data.CinemagicContext context)
        {
            _context = context;
        }

        public Movie Movie { get; set; } = default!;
        public IList<Comment> MovieComments { get; set; } = new List<Comment>();

        [BindProperty]
        public Comment NewComment { get; set; }

        // חדש: האם הסרט כבר נקנה
        public bool IsPurchased { get; set; } = false;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.MovieID == id);
            if (movie == null)
                return NotFound();

            Movie = movie;

            // בדיקה אם המשתמש מחובר וקנה את הסרט
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (!string.IsNullOrEmpty(userIdStr) && int.TryParse(userIdStr, out int userId))
            {
                IsPurchased = await _context.Purchases
                    .AnyAsync(p => p.MemberID == userId && p.MovieID == movie.MovieID);
            }

            // Load comments for this movie
            MovieComments = await _context.Comments
                .Where(c => c.MovieID == id)
                .OrderByDescending(c => c.CommentDate)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Movie == null || Movie.MovieID == 0)
            {
                return NotFound();
            }

            var relatedComments = await _context.Comments
                .Where(c => c.MovieID == Movie.MovieID)
                .ToListAsync();

            _context.Comments.RemoveRange(relatedComments);

            var movieToDelete = await _context.Movies.FindAsync(Movie.MovieID);
            if (movieToDelete != null)
            {
                _context.Movies.Remove(movieToDelete);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostDeleteCommentAsync(int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return RedirectToPage(new { id = comment.MovieID });
        }
    }
}
