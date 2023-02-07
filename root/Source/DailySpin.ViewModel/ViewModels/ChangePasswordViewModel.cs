using System.ComponentModel.DataAnnotations;

namespace DailySpin.ViewModel.ViewModels
{
    public class ChangePasswordViewModel
    {
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        [MinLength(6, ErrorMessage = "Пароль должен быть больше или равен 6 символов")]
        public string NewPassword { get; set; }
    }
}