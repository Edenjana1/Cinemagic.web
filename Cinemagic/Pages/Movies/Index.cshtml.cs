﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Cinemagic.Data;
using Cinemagic.Models;

namespace Cinemagic.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly Cinemagic.Data.CinemagicContext _context;

        public IndexModel(Cinemagic.Data.CinemagicContext context)
        {
            _context = context;
        }

        public IList<Movie> Movie { get;set; } = default!;

        public async Task OnGetAsync(string SearchString)
        {
            IQueryable<Movie> MovieID = from s in _context.Movies select s;

            if (!string.IsNullOrEmpty(SearchString))
            {
                MovieID = MovieID.Where(s => s.MovieName.Contains(SearchString));
            }

            Movie = await MovieID.ToListAsync();
            //Movie = await _context.Movies.ToListAsync();
        }
    }
}
