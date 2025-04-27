namespace Cinemagic.Models
{
    public enum SerieGenre
    {
        Drama, Horror, Thriller, Romantic, Comady, SciFi, Action, Crime, Fantasy, Family,
    }
    public class Serie
    {
        public int SerieID { get; set; }
        public string Image { get; set; }
        public string SerieName { get; set; }
        public int SeasonNum { get; set; }
        public SerieGenre? SerieGenre { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string SerieDescription { get; set; }
        public string AgeRate { get; set; }
        public int SeriePrice { get; set; }

        public ICollection<Purchase> Purchases { get; set; }
    }
}
