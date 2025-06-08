using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cinemagic.Pages.Cupons
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public Coupon Coupon { get; set; } = new Coupon();

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();


            CouponStore.Coupons.Add(Coupon);

            return RedirectToPage("/Cupons/Index");
        }
    }

    public class Coupon
    {
        public string Code { get; set; } = "";
        public string Description { get; set; } = "";
        public int Discount { get; set; }

        // תאריך תפוגה
        public DateTime ExpiryDate { get; set; }
    }

    public static class CouponStore
    {
        public static List<Coupon> Coupons { get; } = new List<Coupon>();
    }
}
