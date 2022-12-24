using System.ComponentModel.DataAnnotations;

namespace DailySpin.Website.Models
{
    public class UserAccount
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmed { get; set; }
        public int Balance { get; set; }
        public string Image { get; set; }
        
    }
}
