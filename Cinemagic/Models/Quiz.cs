namespace Cinemagic.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        public string Title { get; set; } // שם החידון
        public List<Question> Questions { get; set; } = new List<Question>();
    }
    public class Question
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public List<string> Options { get; set; }
        public int CorrectOptionIndex { get; set; }
    }


}
