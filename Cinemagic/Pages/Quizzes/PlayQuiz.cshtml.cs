using Cinemagic.Data;
using Cinemagic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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
        public int Id { get; set; }

        public Quiz Quiz { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CurrentQuestionIndex { get; set; } = 0;

        [BindProperty(SupportsGet = true)]
        public List<int> SelectedOptions { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int? LastSelectedOptionIndex { get; set; }

        public int? LastSelectedOptionCorrectIndex { get; set; }

        public bool IsAnswerChecked =>
            LastSelectedOptionIndex != null && CurrentQuestionIndex < Quiz?.Questions.Count;

        public bool IsQuizFinished { get; set; } = false;

        public int Score { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(q => q.Id == Id);

            if (Quiz == null)
                return NotFound();

            // חישוב ניקוד בסיום
            if (CurrentQuestionIndex >= Quiz.Questions.Count)
            {
                IsQuizFinished = true;
                Score = 0;
                for (int i = 0; i < Quiz.Questions.Count; i++)
                {
                    if (SelectedOptions.Count > i && SelectedOptions[i] == Quiz.Questions[i].CorrectOptionIndex)
                        Score++;
                }
            }

            if (IsAnswerChecked && CurrentQuestionIndex >= 0 && CurrentQuestionIndex < Quiz.Questions.Count)
            {
                var question = Quiz.Questions[CurrentQuestionIndex];
                LastSelectedOptionCorrectIndex = question.CorrectOptionIndex;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int selectedOption)
        {
            Quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(q => q.Id == Id);

            if (Quiz == null)
                return NotFound();

            var correctIndex = Quiz.Questions[CurrentQuestionIndex].CorrectOptionIndex;

            LastSelectedOptionIndex = selectedOption;
            LastSelectedOptionCorrectIndex = correctIndex;

            SelectedOptions.Add(selectedOption);

            // Redirect ל-GET עם הפרמטרים כדי להציג התוצאה עם צבעים
            return RedirectToPage(new
            {
                Id,
                CurrentQuestionIndex,
                LastSelectedOptionIndex,
                SelectedOptions
            });
        }

        public IActionResult OnPostNext()
        {
            CurrentQuestionIndex++;
            LastSelectedOptionIndex = null;

            return RedirectToPage(new
            {
                Id,
                CurrentQuestionIndex,
                SelectedOptions
            });
        }
    }
}
