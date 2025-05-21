using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Cinemagic.Models; // מודל Member
using Microsoft.AspNetCore.Http; // לגישה לסשן


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

        public IActionResult OnPost()
        {
            var member = _context.Members.FirstOrDefault(m => m.IdintityCard == IdintityCard);

            if (member != null)
            {
                // שמירת מזהה המשתמש בסשן
                HttpContext.Session.SetString("UserId", member.MemberID.ToString());
                HttpContext.Session.SetString("UserType", "Member");

                return RedirectToPage("/MembersUser/DetailsUser", new { id = member.MemberID });
            }
            else
            {
                ErrorMessage = "ID not found, please create a new member.";
                return Page();
            }
        }
    }
}
