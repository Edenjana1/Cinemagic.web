using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Cinemagic.Data;
using Cinemagic.Models;

namespace Cinemagic.Pages.Quizzes
{
    public class DetailsModel : PageModel
    {
        private readonly CinemagicContext _context;

        public DetailsModel(CinemagicContext context)
        {
            _context = context;
        }

        public Quiz Quiz { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            // טען את ה-Quiz כולל השאלות המשויכות אליו
            Quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (Quiz == null)
                return NotFound();

            return Page();
        }
    }
}
