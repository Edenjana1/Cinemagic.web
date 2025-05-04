using Microsoft.AspNetCore.Identity;
using System.Drawing;

namespace Cinemagic.Models
{
    public enum Gender
    {
        Male, Female,
    }

    public class Member()
    {
        public int MemberID { get; set; }
        public string Image { get; set; }
        public int IdintityCard { get; set; }
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public Gender? Gender { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }


        public ICollection<Purchase> Purchases { get; set; }
    }
}
