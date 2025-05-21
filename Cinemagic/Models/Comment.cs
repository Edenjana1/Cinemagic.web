namespace Cinemagic.Models
{
    public class Comment
    {
        public int CommentID { get; set; }
        public string Content { get; set; }
        public DateTime CommentDate { get; set; } = DateTime.Now;

        // שדה מזהה של משתמש שכתב את התגובה (מזהה מספרי, תואם ל-MemberID)
        public int? UserID { get; set; }

        // ניווט למשתמש שכתב את התגובה
        public Member? User { get; set; }

        // קישורים אופציונליים לסרט או סדרה
        public int? MovieID { get; set; }
        public Movie? Movie { get; set; }

        public int? SerieID { get; set; }
        public Serie? Serie { get; set; }
    }

}
