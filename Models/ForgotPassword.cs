using System.ComponentModel.DataAnnotations;

namespace SanTech.Models
{
    public class ForgotPassword
    {
        [Required(ErrorMessage = "Это обязательное поле")]
        [EmailAddress(ErrorMessage = "Неверно введён email адрес")]
        public string Email { get; set; }
    }
}
