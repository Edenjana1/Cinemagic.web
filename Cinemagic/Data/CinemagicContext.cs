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
        public DbSet<Purchase> Purchases { get; set; }
    }
}
