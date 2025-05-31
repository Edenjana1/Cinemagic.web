using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Cinemagic.Data;  // ���� ��� �� ������� �� ������
using Cinemagic.Models;  // ���� ����� �� Member

namespace Cinemagic.Pages
{
    public class IndexModel : PageModel
    {
        private readonly CinemagicContext _context;

        public IndexModel(CinemagicContext context)
        {
            _context = context;
        }

        // ����� ������ ���� ����
        public IActionResult OnPost()
        {
            HttpContext.Session.SetString("UserType", "Admin");
            return RedirectToPage("/Index");
        }

        // ����� ����� ����
        public IActionResult OnPostLogoutAdmin()
        {
            HttpContext.Session.Remove("UserType");
            return RedirectToPage("/Index");
        }

        public bool IsUserLoggedIn => HttpContext.Session.GetString("UserType") != null;

        // ����� ������ ���� ����
        public IActionResult OnPostMemberLogin(int idintityCard)
        {
            var member = _context.Members.FirstOrDefault(m => m.IdintityCard == idintityCard);

            if (member != null)
            {
                HttpContext.Session.SetString("UserType", "Member");

                // ������ ��� ������ ���� �� ID �-URL
                return RedirectToPage("/Movies/Index", new { id = member.MemberID });
            }
            else
            {
                // ����� �� ������ �-TempData
                TempData["ErrorMessage"] = "����� �� ���� ������, ���� �����";

                // ������ ����� ���� (Index)
                return RedirectToPage("/Index");
            }
        }

        // ����� ����� ����
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