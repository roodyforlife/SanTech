using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SanTech.Models
{
    public class Basket
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public Product Product { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
        public int NumberOfProduct { get; set; }
    }
}
