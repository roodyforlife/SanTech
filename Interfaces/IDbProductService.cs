using System.Collections.Generic;
using SanTech.Models;
using SanTech.Models.ViewModels;

namespace SanTech.Interfaces
{
    public interface IDbProductService
    {
        public void Add(ProductViewModel product);
        public List<Product> GetAll();
        public IEnumerable<Product> Get(SearchViewModel search);
        public IEnumerable<Product> GetProductsInRange(int from, int count, IEnumerable<Product> products);
        public Product Get(int id);
        public void DeleteProduct(int productId);
        public void RedactProduct(ProductViewModel newProduct, int productId);
    }
}
