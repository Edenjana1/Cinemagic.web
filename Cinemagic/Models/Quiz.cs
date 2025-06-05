using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinemagic.Models
{
    public class Quiz
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public List<Question> Questions { get; set; } = new List<Question>();
    }

    public class Question
    {
        public int Id { get; set; }

        [Required]
        public string QuestionText { get; set; }

        // שמירת האופציות כמחרוזת מופרדת בפסיקים, נשתמש ב-NotMapped לתמיכה בקוד
        [NotMapped]
        public string OptionsSerialized { get; set; }

        [NotMapped]
        public List<string> Options
        {
            get => OptionsSerialized?.Split("|||").ToList() ?? new List<string>();
            set => OptionsSerialized = string.Join("|||", value);
        }

        public int CorrectOptionIndex { get; set; }

        // Foreign key to Quiz
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }
    }
}