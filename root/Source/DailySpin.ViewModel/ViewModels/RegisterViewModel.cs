using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace DailySpin.ViewModel.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The Nickname field should have a maximum of 255 characters")]
        [Display(Name = "Nickname")]
        public string Nickname { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Enter password")]
        [MinLength(6, ErrorMessage = "Password leght must be higher then 5 symbols")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm password")]
        [Compare("Password", ErrorMessage = "Passwords don't match")]
        public string PasswordConfirm { get; set; }

        [Display(Name = "Avatar image")]
        [Required]
        public IFormFile Image { get; set; }
    }
}