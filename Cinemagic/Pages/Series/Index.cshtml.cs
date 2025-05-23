﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Cinemagic.Data;
using Cinemagic.Models;

namespace Cinemagic.Pages.Series
{
    public class IndexModel : PageModel
    {
        private readonly Cinemagic.Data.CinemagicContext _context;

        public IndexModel(Cinemagic.Data.CinemagicContext context)
        {
            _context = context;
        }

        public IList<Serie> Serie { get;set; } = default!;

        public async Task OnGetAsync(string SearchString)
        {
            IQueryable<Serie> SerieID = from s in _context.Series select s;

            if (!string.IsNullOrEmpty(SearchString))
            {
                SerieID = SerieID.Where(s => s.SerieName.Contains(SearchString));
            }

            Serie = await SerieID.ToListAsync();
            //Serie = await _context.Series.ToListAsync();
        }
    }
}
