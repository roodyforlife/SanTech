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
        public void Add(CreateProduct createProduct)
        {
            var image = fileService.FromImageToByte(createProduct.UploadedFile);
            Product product = new Product(createProduct.Title, createProduct.Desc, createProduct.SaleProcent, createProduct.BonusNumber, createProduct.Cost, image);
            db.Products.Add(product);
            db.SaveChanges();
        }

        public void AddThereAre()
        {
            for (int i = 0; i < 1000; i++)
            {
                db.Products.Add(new Product("Title" + i, "Описание", i, i, 400 + i, new byte[] { 1, 34, 2, 54, 3, 35, 45, }));
            }
            db.SaveChanges();
        }

        public Product Get(int Id)
        {
            return db.Products.ToList().FirstOrDefault(x => x.Id == Id);
        }

        public IEnumerable<Product> GetAll()
        {
            return db.Products.ToList();
        }

        public IEnumerable<Product> GetProductsInRange(int from, int count)
        {
            if (from < 0 || count <= 0)
                throw new ArgumentOutOfRangeException();
            return db.Products.Skip(from).Take(count);
        }
    }
}
