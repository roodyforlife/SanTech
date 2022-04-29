using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanTech.Models
{
    public class Basket
    {
        public int Id { get; set; }
        public List<Product> Products { get; set; }
    }
}
