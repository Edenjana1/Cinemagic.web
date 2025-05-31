using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Cinemagic.Data;  // נניח שיש לך קונטקסט של נתונים
using Cinemagic.Models;  // נניח שמודל של Member

namespace Cinemagic.Pages
{
    public class IndexModel : PageModel
    {
        private readonly CinemagicContext _context;

        public IndexModel(CinemagicContext context)
        {
            _context = context;
        }

        // פעולה להיכנס למצב מנהל
        public IActionResult OnPost()
        {
            HttpContext.Session.SetString("UserType", "Admin");
            return RedirectToPage("/Index");
        }

        // יציאה ממעמד מנהל
        public IActionResult OnPostLogoutAdmin()
        {
            HttpContext.Session.Remove("UserType");
            return RedirectToPage("/Index");
        }

        public bool IsUserLoggedIn => HttpContext.Session.GetString("UserType") != null;

        // פעולה להיכנס למצב מנוי
        public IActionResult OnPostMemberLogin(int idintityCard)
        {
            var member = _context.Members.FirstOrDefault(m => m.IdintityCard == idintityCard);

            if (member != null)
            {
                HttpContext.Session.SetString("UserType", "Member");

                // הפנייה לדף פרופיל מנוי עם ID ב-URL
                return RedirectToPage("/Movies/Index", new { id = member.MemberID });
            }
            else
            {
                // שמירה של ההודעה ב-TempData
                TempData["ErrorMessage"] = "משתמש לא נמצא במערכת, הרשם כמנוי";

                // הפנייה לעמוד הבית (Index)
                return RedirectToPage("/Index");
            }
        }

        // יציאה ממעמד מנוי
        public IActionResult OnPostLogoutMember()
        {
            HttpContext.Session.Remove("UserType");
            return RedirectToPage("/Index");
        }

        public void OnGet()
        {
        }
        
    }
}