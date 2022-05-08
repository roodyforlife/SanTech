using Microsoft.EntityFrameworkCore;
using SanTech.Interfaces;
using SanTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanTech.Services
{
    public class DbProductService : IDbProductService
    {
        private readonly ApplicationContext db;
        private readonly IFileService fileService;
        public DbProductService(ApplicationContext db, IFileService fileService)
        {
            this.db = db;
            this.fileService = fileService;
        }
        public void Add(ProductViewModel createProduct)
        {
            var image = fileService.FromImageToByte(createProduct.UploadedFile);
            Product product = new Product(createProduct.Title, createProduct.Desc, createProduct.SaleProcent, createProduct.BonusNumber, createProduct.Cost, image);
            db.Products.Add(product);
            db.SaveChanges();
        }

        public void AddThereAre()
        {
            for (int i = 0; i < 100; i++)
            {
                db.Products.Add(new Product("Title" + i, "Описание", i, i, 400 + i, new byte[] { 1, 34, 2, 54, 3, 35, 45, }));
            }
            db.SaveChanges();
        }

        public Product Get(int Id)
        {
            return db.Products.Include(x => x.Comments).ThenInclude(x => x.SubComments).ThenInclude(x => x.User).ToList().FirstOrDefault(x => x.Id == Id);
        }

        public IEnumerable<Product> GetAll()
        {
            return db.Products.Include(x => x.Comments).ToList();
        }

        public IEnumerable<Product> GetProductsInRange(int from, int count)
        {
            if (from < 0 || count <= 0)
                throw new ArgumentOutOfRangeException();
            return db.Products.Skip(from).Take(count);
        }
        public void DeleteProduct(int productId)
        {
            var product = db.Products.ToList().FirstOrDefault(x => x.Id == productId);
            var basket = db.Baskets.ToList().Where(x => x.Product.Id == productId);
            db.Baskets.RemoveRange(basket);
            db.Products.Remove(product);
            db.SaveChanges();
        }
        public void RedactProduct(ProductViewModel newProduct, int productId)
        {
            var product = db.Products.ToList().FirstOrDefault(x => x.Id == productId);
            if(newProduct.UploadedFile is not null)
            product.Image = fileService.FromImageToByte(newProduct.UploadedFile);
            product.Title = newProduct.Title;
            product.Desc = newProduct.Desc ?? product.Desc;
            product.Cost = newProduct.Cost;
            product.SaleProcent = newProduct.SaleProcent;
            product.BonusNumber = newProduct.BonusNumber;
            product.IsNotAvailable = newProduct.IsNotAvailable;
            db.SaveChanges();
        }
    }
}
