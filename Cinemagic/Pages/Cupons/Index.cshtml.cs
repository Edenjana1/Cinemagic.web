using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http; // ודא שהתווסף

namespace Cinemagic.Pages.Coupons
{
    public class IndexModel : PageModel
    {
        public List<Coupon> Coupons { get; set; } = new List<Coupon>();
        public bool IsAdmin { get; set; }

        public void OnGet()
        {
            IsAdmin = HttpContext.Session.GetString("IsAdmin") == "true";
            Coupons = CouponStore.Coupons.ToList();
        }

        public IActionResult OnPostDelete(string code)
        {
            var isAdmin = HttpContext.Session.GetString("IsAdmin") == "true";



            var couponToRemove = CouponStore.Coupons.FirstOrDefault(c => c.Code == code);
            if (couponToRemove != null)
            {
                CouponStore.Coupons.Remove(couponToRemove);
            }

            return RedirectToPage(); // חזרה לעמוד הקופונים לאחר מחיקה
        }
    }
}
