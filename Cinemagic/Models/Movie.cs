namespace Cinemagic.Models
{
    public enum MovieGenre
    {
        Drama, Horror, Thriller, Romantic, Comady, SciFi, Action, Crime, Fantasy, Family,
    }
    public class Movie
    {
        public int MovieID { get; set; }

        public string Image { get; set; }
        public string MovieName { get; set; }
        public MovieGenre? MovieGenre { get; set; }
        public string MovieDescription { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string AgeRate { get; set; }
        public int MoviePrice { get; set; }

        public ICollection<Purchase> Purchases { get; set; }
    }
}
