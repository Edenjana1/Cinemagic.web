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
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Purchase> Purchases { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany() 
                .HasForeignKey(c => c.UserID)
                .OnDelete(DeleteBehavior.SetNull); 

            modelBuilder.Entity<Question>()
                .HasOne(q => q.Quiz)
                .WithMany(qz => qz.Questions)
                .HasForeignKey(q => q.QuizId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Purchase>()
                .Property(p => p.Total)
                .HasColumnType("decimal(18,2)");
        }
    }
}
