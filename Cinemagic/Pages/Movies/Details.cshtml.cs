using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Cinemagic.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http; // For accessing session
using System;

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

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.MovieID == id);
            if (movie == null)
                return NotFound();

            Movie = movie;

            // Load comments for this movie
            MovieComments = await _context.Comments
                .Where(c => c.MovieID == id)
                //.Include(c => c.User)
                .OrderByDescending(c => c.CommentDate)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!ModelState.IsValid || id == null)
            {
                return Page();
            }

            NewComment.MovieID = id.Value;
            NewComment.CommentDate = DateTime.Now;

            // Try to read the UserId from the session
            //var userIdStr = HttpContext.Session.GetString("UserId");
            //if (int.TryParse(userIdStr, out int userId))
            //{
            //    NewComment.UserID = userId;
            //}

            _context.Comments.Add(NewComment);
            await _context.SaveChangesAsync();

            return RedirectToPage();
            //return RedirectToPage(new { id });
        }
    }
}
