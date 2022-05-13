using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SanTech.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Это обязательное поле")]
        [StringLength(12, MinimumLength = 2, ErrorMessage = "Имя должно быть от 2 до 13 символов")]
        public string Name { get; set; }
        //[Required(ErrorMessage = "Это обязательное поле")]
        //[RegularExpression(@"[A-Za-z0-9_]+", ErrorMessage = "Логин должен состоять из латинский букв, цифр и символа '_'")]
        //[StringLength(12, MinimumLength = 5, ErrorMessage = "Логин должен быть от 5 до 12 символов")]
        //public string Login { get; set; }
        [Required(ErrorMessage = "Это обязательное поле")]
        //[StringLength(20, MinimumLength = 6, ErrorMessage = "Пароль должен быть от 6 до 20 символов")]
       //[RegularExpression(@"[A-Za-z0-9_]+", ErrorMessage = "Пароль должен состоять из латинский букв, цифр и символа '_'")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Это обязательное поле")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string PasswordConfirm { get; set; }
        [Required(ErrorMessage = "Это обязательное поле")]
        [EmailAddress(ErrorMessage = "Неверно введён email адрес")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Это обязательное поле")]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Номер телефона должен состоять из цифр")]
        public string Phone { get; set; }
        public int Bonus { get; set; }
        public bool IsAdmin { get; set; }
        public byte[] Avatar { get; set; }
        public List<Basket> Basket { get; set; } = new List<Basket>();
        public List<Favorite> Favorites { get; set; } = new List<Favorite>();
    }
}
