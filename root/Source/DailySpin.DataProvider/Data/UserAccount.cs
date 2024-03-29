﻿using DailySpin.DataProvider.Enums;
using System.ComponentModel.DataAnnotations;

namespace DailySpin.DataProvider.Data
{
    public class UserAccount
    {
        [Key]
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ulong Balance { get; set; }
        public byte[]? Image { get; set; }
        public Role Role { get; set; }
    }
}
