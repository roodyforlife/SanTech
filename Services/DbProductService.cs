using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SanTech.Interfaces;
using SanTech.Models;
using SanTech.Models.ViewModels;

namespace SanTech.Services
{
    public class DbProductService : IDbProductService
    {
        private readonly DataBaseContext _db;
        private readonly IFileService _fileService;
        public DbProductService(DataBaseContext db, IFileService fileService)
        {
            _db = db;
            _fileService = fileService;
        }

        public void Add(ProductViewModel createProduct)
        {
            var image = _fileService.FromImageToByte(createProduct.UploadedFile);
            Product product = new Product(createProduct.Title, createProduct.Desc, createProduct.SaleProcent, createProduct.BonusNumber, createProduct.Cost, image, createProduct.CategoryId);
            _db.Products.Add(product);
            _db.SaveChanges();
        }

        public Product Get(int id)
        {
            return _db.Products.Include(x => x.Comments).ThenInclude(x => x.User).Include(x => x.Comments).ThenInclude(x => x.SubComments).ToList().FirstOrDefault(x => x.Id == id);
        }

        public List<Product> GetAll()
        {
            var products = _db.Products.Include(x => x.Comments).ToList();
            return products;
        }

        public IEnumerable<Product> GetProductsInRange(int from, int count, IEnumerable<Product> products)
        {
            if (from < 0 || count <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            return products.Skip(from).Take(count);
        }

        public void DeleteProduct(int productId)
        {
            var product = _db.Products.ToList().FirstOrDefault(x => x.Id == productId);
            var basket = _db.Baskets.ToList().Where(x => x.Product.Id == productId);
            var subComments = _db.SubComments.ToList().Where(x => x.Comment.Product.Id == productId);
            var comments = _db.Comments.ToList().Where(x => x.Product.Id == productId);
            var favorites = _db.Favorites.ToList().Where(x => x.Product.Id == productId);
            _db.Baskets.RemoveRange(basket);
            _db.SubComments.RemoveRange(subComments);
            _db.Comments.RemoveRange(comments);
            _db.Favorites.RemoveRange(favorites);
            _db.Products.Remove(product);
            _db.SaveChanges();
        }

        public void RedactProduct(ProductViewModel newProduct, int productId)
        {
            var product = _db.Products.ToList().FirstOrDefault(x => x.Id == productId);
            if (newProduct.UploadedFile is not null)
            {
                product.Image = _fileService.FromImageToByte(newProduct.UploadedFile);
            }

            product.Title = newProduct.Title;
            product.Desc = newProduct.Desc ?? product.Desc;
            product.Cost = newProduct.Cost;
            product.SaleProcent = newProduct.SaleProcent;
            product.BonusNumber = newProduct.BonusNumber;
            product.IsNotAvailable = newProduct.IsNotAvailable;
            _db.SaveChanges();
        }

        public IEnumerable<Product> Get(SearchViewModel search)
        {
            var products = _db.Products.Include(x => x.Comments).AsEnumerable();
            if (search.Category != 0)
            {
                products = products.ToList().Where(x => x.CategoryId == search.Category);
            }

            if (search.CostTo != 0)
            {
                products = products.Where(x => x.Cost * (100 - x.SaleProcent) / 100 >= search.CostFrom && x.Cost * (100 - x.SaleProcent) / 100 <= search.CostTo);
            }

            if (!string.IsNullOrEmpty(search.SearchInput))
            {
                products = products.Where(x => x.Title.Contains(search.SearchInput, StringComparison.OrdinalIgnoreCase));
            }

            switch (search.Sort)
            {
                case "CostDesc":
                    products = products.OrderByDescending(x => x.Cost * (100 - x.SaleProcent) / 100);
                    break;
                case "CostAsc":
                    products = products.OrderBy(x => x.Cost * (100 - x.SaleProcent) / 100);
                    break;
                case "NameDesc":
                    products = products.OrderByDescending(x => x.Title);
                    break;
                case "NameAsc":
                    products = products.OrderBy(x => x.Title);
                    break;
                case "NewDesc":
                    products = products.OrderByDescending(x => x.Id);
                    break;
                case "NewAsc":
                    products = products.OrderBy(x => x.Id);
                    break;
                default:
                    products = products.OrderByDescending(x => x.Id);
                    break;
            }

            return products;
        }
    }
}
