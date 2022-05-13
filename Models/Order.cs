using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SanTech.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Это обязательное поле")]
        [StringLength(12, MinimumLength = 2, ErrorMessage = "Имя должно быть от 2 до 13 символов")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Это обязательное поле")]
        [StringLength(12, MinimumLength = 2, ErrorMessage = "Фамилия должна быть от 2 до 20 символов")]
        public string SecondName { get; set; }
        [Required(ErrorMessage = "Это обязательное поле")]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Номер телефона должен состоять из цифр")]
        public int Phone { get; set; }
        [Required(ErrorMessage = "Это обязательное поле")]
        [EmailAddress(ErrorMessage = "Неверно введён email адрес")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Это обязательное поле")]
        public string City { get; set; }
        [Required(ErrorMessage = "Это обязательное поле")]
        public string Delivery { get; set; }
        [Required(ErrorMessage = "Это обязательное поле")]
        public string Address { get; set; }
        public bool WriteOffBonuses { get; set; }
        public List<Basket> Basket = new List<Basket>();
        public User User { get; set; }
        public int TotalCost { get; set; }
        public string OrderNumber = DateTime.Now.ToString("ddMMyyyyHHmmss");
    }
}
