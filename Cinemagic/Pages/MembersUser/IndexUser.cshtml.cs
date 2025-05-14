using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Cinemagic.Data;
using Cinemagic.Models;

namespace Cinemagic.Pages.MembersUser
{
    public class IndexUserModel : PageModel
    {
        private readonly Cinemagic.Data.CinemagicContext _context;

        public IndexUserModel(Cinemagic.Data.CinemagicContext context)
        {
            _context = context;
        }

        public IList<Member> Member { get;set; } = default!;

        public async Task OnGetAsync(string SearchString)
        {
            IQueryable<Member> MemberID = from s in _context.Members select s;

            if (!string.IsNullOrEmpty(SearchString))
            {
                MemberID = MemberID.Where(s => s.LastName.Contains(SearchString) || s.FirstMidName.Contains(SearchString));
            }

            Member = await MemberID.ToListAsync();
            //Member = await _context.Members.ToListAsync();
        }
    }
}
