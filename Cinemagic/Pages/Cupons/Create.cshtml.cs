using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cinemagic.Pages.Coupons
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public Coupon Coupon { get; set; } = new Coupon();

        public IActionResult OnGet()
        {
            var isAdmin = HttpContext.Session.GetString("IsAdmin") == "true";
            if (!isAdmin)
                return Forbid();

            return Page();
        }

        public IActionResult OnPost()
        {
            var isAdmin = HttpContext.Session.GetString("IsAdmin") == "true";
            if (!isAdmin)
                return Forbid();

            if (!ModelState.IsValid)
                return Page();

            Coupon.ExpiryDate = CouponStore.GetFirstDayOfNextMonth();

            CouponStore.Coupons.Add(Coupon);

            return RedirectToPage("/Coupons/Index");
        }


    }
    public class Coupon
    {
        public string Code { get; set; } = "";
        public string Description { get; set; } = "";

        // תאריך תפוגה — היום הראשון של החודש הבא
        public DateTime ExpiryDate { get; set; }
    }


    public static class CouponStore
    {
        public static DateTime GetFirstDayOfNextMonth()
        {
            var today = DateTime.Today;
            return new DateTime(today.Year, today.Month, 1).AddMonths(1);
        }

        public static List<Coupon> Coupons { get; } = new List<Coupon>
        {
            new Coupon
            {
                Code = "SERIE10",
                Description = "10% הנחה על סדרות",
                ExpiryDate = GetFirstDayOfNextMonth()
            },
            new Coupon
            {
                Code = "MOVIE15",
                Description = "15% הנחה על סרטים",
                ExpiryDate = GetFirstDayOfNextMonth()
            }
        };
    }



}
