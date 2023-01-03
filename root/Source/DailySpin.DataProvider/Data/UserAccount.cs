using DailySpin.DataProvider.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace DailySpin.DataProvider.Data
{
    public class UserAccount : IdentityUser
    {
        public string DisplayName { get; set; }
        public long Balance { get; set; }
        public byte[]? Image { get; set; }
        public Role Role { get; set; }
    }
}
