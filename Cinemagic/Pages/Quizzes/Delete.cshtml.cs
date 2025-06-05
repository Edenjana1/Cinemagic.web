using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Cinemagic.Data;
using Cinemagic.Models;

namespace Cinemagic.Pages.Quizzes
{
    public class DeleteModel : PageModel
    {
        private readonly Cinemagic.Data.CinemagicContext _context;

        public DeleteModel(Cinemagic.Data.CinemagicContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Quiz Quiz { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quiz = await _context.Quizzes.FirstOrDefaultAsync(m => m.Id == id);

            if (quiz == null)
            {
                return NotFound();
            }
            else
            {
                Quiz = quiz;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz != null)
            {
                Quiz = quiz;
                _context.Quizzes.Remove(Quiz);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
