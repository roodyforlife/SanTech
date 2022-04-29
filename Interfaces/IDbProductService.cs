using SanTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanTech.Interfaces
{
    public interface IDbProductService
    {
        public void Add(Product product);
        public IEnumerable<Product> GetAll();
    }
}
