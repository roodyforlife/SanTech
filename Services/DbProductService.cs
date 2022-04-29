﻿using SanTech.Interfaces;
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

        public void AddThereAre()
        {
            for(int i = 0; i < 1000; i++)
            {
                db.Products.Add(new Product("Title" + i, "Описание", i, i, 400 + i, new byte[] { 1, 34, 2, 54, 3, 35, 45, }));
            }
            db.SaveChanges();
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
