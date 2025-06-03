using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Cinemagic.Data;
using Cinemagic.Models;
using Microsoft.EntityFrameworkCore;

namespace Cinemagic.Pages.MembersUser
{
    public class CreateUserModel : PageModel
    {
        private readonly Cinemagic.Data.CinemagicContext _context;

        public CreateUserModel(Cinemagic.Data.CinemagicContext context)
        {
            _context = context;
        }
        public List<SelectListItem> ImageOptions { get; set; } = new List<SelectListItem>();

        public List<SelectListItem> GenderOptions { get; set; } = new List<SelectListItem>();
        public IActionResult OnGet()
        {
            ImageOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Girl", Value = "girl_icon.jpg" },
                new SelectListItem { Text = "Boy", Value = "boy_icon.jpg" },
                new SelectListItem { Text = "Woman", Value = "woman_icon.jpg" },
                new SelectListItem { Text = "Man", Value = "man_icon.jpg" }
            };

            GenderOptions = Enum.GetValues(typeof(Gender))
                .Cast<Gender>()
                .Select(g => new SelectListItem
                {
                    Text = g.ToString(),  // הערך של המגדר (Male, Female)
                    Value = g.ToString()   // שמירת הערך (Male, Female)
                }).ToList();
            return Page();
        }

        [BindProperty]
        public Member Member { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Members.Add(Member);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }


    }
}
