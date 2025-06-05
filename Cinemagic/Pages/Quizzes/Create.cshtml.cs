using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Cinemagic.Data;
using Cinemagic.Models;

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
        public List<Question> Questions { get; set; } = new List<Question>();

        public void OnGet()
        {
            Quiz = new Quiz();
            Questions = new List<Question>();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            foreach (var question in Questions)
            {
                // ודא שה-OptionsSerialized נבנה נכון מה-Options
                if (question.Options != null && question.Options.Any())
                {
                    question.OptionsSerialized = string.Join("|||", question.Options);
                }

                question.Quiz = Quiz;
            }

            Quiz.Questions = Questions;
            _context.Quizzes.Add(Quiz);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
