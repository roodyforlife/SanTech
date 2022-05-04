﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SanTech.Models
{
    public class ResetPassword
    {
        [Required(ErrorMessage = "Это обязательное поле")]
        [EmailAddress(ErrorMessage = "Неверно введён email адрес")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Это обязательное поле")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Это обязательное поле")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
        public string Code { get; set; }

    }
}