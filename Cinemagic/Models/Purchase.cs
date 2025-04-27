namespace Cinemagic.Models
{
    public class Purchase
    {
        public int PurchaseID { get; set; }
        public int? MovieID { get; set; }
        public int? SerieID { get; set; }
        public int? MemberID { get; set; }
        public string? Email { get; set; }
        public DateTime? PurchaseDate { get; set; } = DateTime.Now;
        public decimal Total { get; set; }
        public decimal CalculateTotalPrice()
        {
            decimal moviePrice = Movies?.MoviePrice ?? 0;  // אם לא קיים סרט, שים 0
            decimal seriePrice = Series?.SeriePrice ?? 0;  // אם לא קיימת סדרה, שים 0
            return moviePrice + seriePrice;
        }

        // עדכון המחיר הכולל
        public void UpdateTotal()
        {
            Total = CalculateTotalPrice();
        }

        public Movie Movies { get; set; }
        public Serie Series { get; set; }
        public Member Members { get; set; }

    }
}
