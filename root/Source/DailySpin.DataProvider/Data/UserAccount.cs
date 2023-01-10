using DailySpin.DataProvider.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace DailySpin.DataProvider.Data
{
    public class UserAccount
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ulong Balance { get; set; }
        public byte[]? Image { get; set; }
        public Role Role { get; set; }
    }
}
