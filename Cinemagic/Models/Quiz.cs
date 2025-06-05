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

        // זהו השדה שישמר בפועל במסד הנתונים
        public string OptionsSerialized { get; set; }

        // שדה מחושב – לא נשמר בבסיס הנתונים
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