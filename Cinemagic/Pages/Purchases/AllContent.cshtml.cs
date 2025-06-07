using Cinemagic.Data;
using Cinemagic.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinemagic.Pages.Purchases
{
    public class AllContentModel : PageModel
    {
        private readonly CinemagicContext _context;

        public AllContentModel(CinemagicContext context)
        {
            _context = context;
        }

        public List<Movie> AllMovies { get; set; } = new();
        public List<Serie> AllSeries { get; set; } = new();

        public Dictionary<int, string> MovieUrls = new()
        {
            { 1, "https://www6.f2movies.to/movie/wicked-117565" },
            { 2, "https://www6.f2movies.to/movie/dune-6752" },
            // הוסף עוד כתובות לסרטים
        };

        public Dictionary<int, string> SeriesUrls = new()
        {
            { 1, "https://www6.f2movies.to/tv/game-of-thrones-39539" },
            { 2, "https://www6.f2movies.to/tv/cobra-kai-38555" },
            // הוסף עוד כתובות לסדרות
        };

        public async Task OnGetAsync()
        {
            AllMovies = await _context.Movies.ToListAsync();
            AllSeries = await _context.Series.ToListAsync();
        }
    }
}
