using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SanTech.Models
{
    public class UserLogin
    {
        [Required(ErrorMessage = "Это обязательное поле")]
        [EmailAddress(ErrorMessage = "Неверно введён email адрес")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Это обязательное поле")]
        //[StringLength(20, MinimumLength = 6, ErrorMessage = "Пароль должен быть от 6 до 20 символов")]
        //[RegularExpression(@"[A-Za-z0-9_]+", ErrorMessage = "Пароль должен состоять из латинский букв, цифр и символа '_'")]
        public string Password { get; set; }
    }
}
