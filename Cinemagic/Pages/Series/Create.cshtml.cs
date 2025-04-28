using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Cinemagic.Data;
using Cinemagic.Models;

namespace Cinemagic.Pages.Series
{
    public class CreateModel : PageModel
    {
        private readonly Cinemagic.Data.CinemagicContext _context;

        public CreateModel(Cinemagic.Data.CinemagicContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Serie Serie { get; set; } = default!;

        public List<SelectListItem> ImagesList { get; set; } = new List<SelectListItem>();

        public IActionResult OnGet()
        {
            // טוען את כל התמונות מתוך תיקיית wwwroot/images
            var imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

            if (Directory.Exists(imageDirectory))
            {
                var imageFiles = Directory.GetFiles(imageDirectory, "*.*", SearchOption.TopDirectoryOnly)
                                          .Where(file => file.EndsWith(".jpg") || file.EndsWith(".png") || file.EndsWith(".jpeg"))
                                          .ToList();

                // המרת שם הקובץ ל-SelectListItem עם האפשרות להציג תמונה
                foreach (var file in imageFiles)
                {
                    var fileName = Path.GetFileName(file);
                    ImagesList.Add(new SelectListItem
                    {
                        Text = fileName, // מציג את שם הקובץ
                        //Value = "/images/" + fileName // נתיב התמונה
                    });
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Series.Add(Serie);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

