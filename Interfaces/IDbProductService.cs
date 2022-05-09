using SanTech.Models;
using SanTech.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanTech.Interfaces
{
    public interface IDbProductService
    {
        public void Add(ProductViewModel product);
        public IEnumerable<Product> GetAll();
        public IEnumerable<Product> GetAll(SearchViewModel search);
        public IEnumerable<Product> GetProductsInRange(int from, int count, IEnumerable<Product> products);
        public Product Get(int Id);
        public void AddThereAre();
        public void DeleteProduct(int productId);
        public void RedactProduct(ProductViewModel newProduct, int productId);
    }
}
