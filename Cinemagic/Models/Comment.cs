namespace Cinemagic.Models
{
    public class Comment
    {
        public int CommentID { get; set; }
        public string Content { get; set; }
        public DateTime CommentDate { get; set; } = DateTime.Now;

        // שדה מזהה של משתמש שכתב את התגובה (אם יש)
        public string? UserID { get; set; }

        // קישורים אופציונליים לסרט או לסדרה
        public int? MovieID { get; set; }
        public Movie? Movie { get; set; }

        public int? SerieID { get; set; }
        public Serie? Serie { get; set; }
    }
}
