using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Cinemagic.Data;
using Cinemagic.Models;

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
        public IList<Comment> MovieComments { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.MovieID == id);
            if (movie == null)
            {
                return NotFound();
            }
            else
            {
                Movie = movie;
            }

            // טוענים תגובות לסרט הנבחר בלבד
            MovieComments = await _context.Comments
                .Where(c => c.MovieID == id)
                .OrderByDescending(c => c.CommentDate)
                .ToListAsync();

            return Page();
        }
        [BindProperty]
        public Comment NewComment { get; set; }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!ModelState.IsValid || id == null)
            {
                return Page();
            }

            NewComment.MovieID = id.Value;
            NewComment.CommentDate = DateTime.Now;

            _context.Comments.Add(NewComment);
            await _context.SaveChangesAsync();

            return RedirectToPage(new { id });
        }

    }
}
