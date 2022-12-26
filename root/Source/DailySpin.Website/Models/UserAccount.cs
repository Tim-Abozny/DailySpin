using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DailySpin.Website.Models
{
    public class UserAccount : IdentityUser
    {
        public string Name { get; set; }
        public long Balance { get; set; }
        public string Image { get; set; }

    }
}
