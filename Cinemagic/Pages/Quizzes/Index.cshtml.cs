using Cinemagic.Data;
using Cinemagic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Cinemagic.Pages.Quizzes
{
    public class IndexModel : PageModel
    {
        private readonly CinemagicContext _context;
        public IndexModel(CinemagicContext context)
        {
            _context = context;
        }
        public List<Quiz> Quizzes { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Quizzes = await _context.Quizzes.ToListAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostDeleteAsync(int? id)
        {
            if (id == null)
                return NotFound();

            // טוען את החידון כולל השאלות
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quiz == null)
                return NotFound();

            // מוחק את כל השאלות
            _context.Questions.RemoveRange(quiz.Questions);

            // מוחק את החידון
            _context.Quizzes.Remove(quiz);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
