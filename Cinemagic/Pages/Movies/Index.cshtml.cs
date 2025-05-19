using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Cinemagic.Data;
using Cinemagic.Models;
using Humanizer.Localisation;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Cinemagic.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly CinemagicContext _context;

        public IndexModel(CinemagicContext context)
        {
            _context = context;
        }

        public IList<Movie> Movie { get; set; } = default!;

        public List<SelectListItem> Genres { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public MovieGenre? Genre { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public List<Movie> MostPurchasedMovies { get; set; } = new();
        public IList<Movie> RecentMovies { get; set; } = default!;

        public List<Movie> KidsMovies { get; set; } = new();
        public async Task OnGetAsync()
        {
            // יצירת רשימת ז'אנרים מתוך ה-enum
            Genres = Enum.GetValues(typeof(MovieGenre))
                .Cast<MovieGenre>()
                .Select(g => new SelectListItem
                {
                    Value = g.ToString(),
                    Text = g.ToString() // אפשר לשנות לעברית
                })
                .ToList();

            var query = _context.Movies.AsQueryable();

            if (Genre.HasValue)
            {
                query = query.Where(m => m.MovieGenre == Genre.Value);
            }

            if (!string.IsNullOrEmpty(SearchString))
            {
                query = query.Where(m => m.MovieName.Contains(SearchString));
            }

            Movie = await query.ToListAsync();

            // --- רשימת הסרטים הכי נרכשים ---
            MostPurchasedMovies = await _context.Purchases
                .Where(p => p.MovieID != null)
                .GroupBy(p => p.MovieID)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .Take(5)
                .Join(_context.Movies,
                    movieId => movieId,
                    movie => movie.MovieID,
                    (movieId, movie) => movie)
                .ToListAsync();
            
            //סרטים חדשים
            DateTime oneYearAgo = DateTime.Now.AddYears(-1);

            RecentMovies = await _context.Movies
                .Where(m => m.ReleaseDate >= oneYearAgo)
                .OrderByDescending(m => m.ReleaseDate)
                .ToListAsync();

            //סרטים לילדים
            KidsMovies = await _context.Movies
                .Where(m => m.AgeRate == "G" || m.AgeRate == "TV-Y" || m.AgeRate == "PG")
                .ToListAsync();
        }

    }
}