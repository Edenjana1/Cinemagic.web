using Cinemagic.Models;
using Microsoft.EntityFrameworkCore;

namespace Cinemagic.Data
{
    public class CinemagicContext : DbContext
    {
        public CinemagicContext(DbContextOptions<CinemagicContext> options) : base(options)
        {
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Serie> Series { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Purchase> Purchases { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // קשר בין תגובה למשתמש (Member)
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany() // אם תוסיף ל-`Member` רשימת תגובות אפשר לשים פה WithMany(m => m.Comments)
                .HasForeignKey(c => c.UserID)
                .OnDelete(DeleteBehavior.SetNull); // כשמשתמש נמחק, התגובה תישאר בלי יוצר

            // קשרים נוספים אפשריים אם תצטרך בהמשך
        }
    }
}
