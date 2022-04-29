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
        public DbProductService(ApplicationContext db)
        {
            this.db = db;
        }
        public void Add(Product product)
        {
            db.Products.Add(product);
            db.SaveChanges();
        }

        public IEnumerable<Product> GetAll()
        {
            return db.Products.ToList();
        }
    }
}
