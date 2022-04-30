using SanTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanTech.Interfaces
{
    public interface IDbProductService
    {
        public void Add(CreateProduct product);
        public IEnumerable<Product> GetAll();
        public IEnumerable<Product> GetProductsInRange(int from, int count);
        public Product Get(int Id);
        public void AddThereAre();
    }
}
