namespace Cinemagic.Models
{
    public class Purchase
    {
        public int PurchaseID { get; set; }
        public int? MovieID { get; set; }
        public int? SerieID { get; set; }
        public int? MemberID { get; set; }
        public int? MemberEmail { get; set; }
        public DateTime PurchaseDate { get; set; }


        public Movie Movies { get; set; }
        public Serie Series { get; set; }
        public Member Members { get; set; }

    }
}
