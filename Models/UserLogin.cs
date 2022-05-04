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
        [RegularExpression(@"[A-Za-z0-9_]+", ErrorMessage = "Логин должен состоять из латинский букв, цифр и символа '_'")]
        [StringLength(12, MinimumLength = 5, ErrorMessage = "Логин должен быть от 5 до 12 символов")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Это обязательное поле")]
        //[StringLength(20, MinimumLength = 6, ErrorMessage = "Пароль должен быть от 6 до 20 символов")]
        //[RegularExpression(@"[A-Za-z0-9_]+", ErrorMessage = "Пароль должен состоять из латинский букв, цифр и символа '_'")]
        public string Password { get; set; }
    }
}
