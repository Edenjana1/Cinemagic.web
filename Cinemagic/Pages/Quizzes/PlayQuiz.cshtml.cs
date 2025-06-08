using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Cinemagic.Data;
using Cinemagic.Models;

namespace Cinemagic.Pages.Quizzes
{
    public class PlayQuizModel : PageModel
    {
        private readonly CinemagicContext _context;

        public PlayQuizModel(CinemagicContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int QuizId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int QuestionIndex { get; set; } = 0;

        public Quiz Quiz { get; set; } = default!;

        public Question CurrentQuestion { get; set; } = default!;

        [BindProperty]
        public int? SelectedOptionIndex { get; set; }

        public bool? IsAnswerCorrect { get; set; } = null;

        public bool IsLastQuestion => QuestionIndex >= Quiz.Questions.Count - 1;

        [TempData]
        public int CorrectAnswersCount { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(q => q.Id == QuizId);

            if (Quiz == null)
                return NotFound();

            if (QuestionIndex < 0 || QuestionIndex >= Quiz.Questions.Count)
                return RedirectToPage("./Index");

            // התחלה מחדש
            if (QuestionIndex == 0)
            {
                CorrectAnswersCount = 0;
                TempData[nameof(CorrectAnswersCount)] = 0;
            }
            if (TempData.ContainsKey(nameof(CorrectAnswersCount)))
            {
                CorrectAnswersCount = (int)TempData[nameof(CorrectAnswersCount)];
                TempData.Keep(nameof(CorrectAnswersCount));
            }


            CurrentQuestion = Quiz.Questions.OrderBy(q => q.Id).ElementAt(QuestionIndex);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(q => q.Id == QuizId);

            if (Quiz == null)
                return NotFound();

            if (QuestionIndex < 0 || QuestionIndex >= Quiz.Questions.Count)
                return RedirectToPage("./Index");

            CurrentQuestion = Quiz.Questions.OrderBy(q => q.Id).ElementAt(QuestionIndex);

            if (SelectedOptionIndex == null)
            {
                ModelState.AddModelError("", "Please select an option.");
                return Page();
            }

            IsAnswerCorrect = (SelectedOptionIndex == CurrentQuestion.CorrectOptionIndex);

            if (IsAnswerCorrect.Value)
            {
                CorrectAnswersCount++;
            }

            TempData[nameof(CorrectAnswersCount)] = CorrectAnswersCount;

            if (Request.Form.ContainsKey("NextQuestion"))
            {
                QuestionIndex++;
                return RedirectToPage(new { QuizId = QuizId, QuestionIndex = QuestionIndex });
            }

            return Page();
        }
    }
}
