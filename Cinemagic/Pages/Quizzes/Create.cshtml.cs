using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Cinemagic.Data;
using Cinemagic.Models;
using Newtonsoft.Json;

namespace Cinemagic.Pages.Quizzes
{
    public class CreateModel : PageModel
    {
        private readonly CinemagicContext _context;

        public CreateModel(CinemagicContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Quiz Quiz { get; set; }

        [BindProperty]
        public List<Question> Questions { get; set; }

        public void OnGet()
        {
            Quiz = new Quiz();
            Questions = new List<Question>();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            Quiz.Questions = Questions;
            _context.Quizzes.Add(Quiz);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
