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
    }
}
