using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Cinemagic.Models; // חשוב לוודא שהמודל של MEMBERS נמצא כאן

namespace Cinemagic.Pages.Login
{
    public class IndexModel : PageModel
    {
        private readonly Cinemagic.Data.CinemagicContext _context;

        public IndexModel(Cinemagic.Data.CinemagicContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int IdintityCard { get; set; }

        public string ErrorMessage { get; set; }

        // פעולה שמטפלת בהתחברות
        public IActionResult OnPost()
        {
            var member = _context.Members.FirstOrDefault(m => m.IdintityCard == IdintityCard);

            if (member != null)
            {
                // אם המשתמש נמצא, נשמור את הנתונים בסשן ונוודא שהוא מחובר כמשתמש
                HttpContext.Session.SetString("UserType", "Member");

                // הפניית המשתמש לדף הפרופיל שלו (לא לדף הבית)
                return RedirectToPage("/MembersUser/DetailsUser", new { id = member.MemberID });
            }
            else
            {
                // אם המשתמש לא נמצא, נציג הודעת שגיאה
                ErrorMessage = "ID not found, please create a new member.";
                return Page(); // נשארים באותו דף
            }
        }
    }
}
