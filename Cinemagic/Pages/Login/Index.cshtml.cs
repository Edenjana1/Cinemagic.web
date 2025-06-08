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

        [BindProperty]
        public string PhoneNumber { get; set; }

        public string ErrorMessage { get; set; }

        public IActionResult OnPost()
        {
            var member = _context.Members
                .FirstOrDefault(m => m.IdintityCard == IdintityCard && m.Phone == PhoneNumber);

            if (member != null)
            {
                HttpContext.Session.SetString("UserId", member.MemberID.ToString());
                HttpContext.Session.SetString("FullName", member.FirstMidName + " " + member.LastName);
                HttpContext.Session.SetString("UserType", "Member");

                return RedirectToPage("/Movies/Index", new { id = member.MemberID });
            }
            else
            {
                ErrorMessage = "ID or phone number incorrect. Please try again or create a new account.";
                return Page();
            }
        }
    }
}
