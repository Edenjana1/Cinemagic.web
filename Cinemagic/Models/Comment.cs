namespace Cinemagic.Models
{
    public class Comment
    {
        public int CommentID { get; set; }
        public string Content { get; set; }
        public DateTime CommentDate { get; set; } = DateTime.Now;
        public int? UserID { get; set; }
        public Member? User { get; set; }

        public int? MovieID { get; set; }
        public Movie? Movie { get; set; }

        public int? SerieID { get; set; }
        public Serie? Serie { get; set; }
    }

}
