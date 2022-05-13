using Microsoft.EntityFrameworkCore;
using SanTech.Interfaces;
using SanTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SanTech.Services
{
    public class DbBasketService : IDbBasketService
    {
        private readonly ApplicationContext db;
        private readonly IDbUserService dbUserService;
        private readonly IDbProductService dbProductService;
        public DbBasketService(ApplicationContext db, IDbUserService dbUserService, IDbProductService dbProductService)
        {
            this.db = db;
            this.dbUserService = dbUserService;
            this.dbProductService = dbProductService;
        }

        public bool Add(string email, int productId)
        {
            var user = dbUserService.Get(email);
            var product = dbProductService.Get(productId);
            if (Get(productId, user.Id) is null)
            {
                Basket basket = new Basket { Product = product, User = user, NumberOfProduct = 1 };
                db.Baskets.Add(basket);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public void Delete(int basketId)
        {
            var basket = Get(basketId);
            db.Baskets.Remove(basket);
            db.SaveChanges();
        }

        public Basket Get(int basketId)
        {
            return db.Baskets.Include(x => x.User).Include(x => x.Product).ToList().FirstOrDefault(x => x.Id == basketId);
        }

        public Basket Get(int productId, int userId)
        {
            return db.Baskets.Include(x => x.Product).Include(x => x.User).ToList().FirstOrDefault(x => x.Product.Id == productId && x.User.Id == userId);
        }

        public List<Basket> Get(string userEmail)
        {
            return db.Baskets.Include(x => x.Product).Include(x => x.User).ToList().Where(x => x.User.Email == userEmail).ToList();
        }

        public void ChangeNumberOfBasket(int basketId, int inputValue)
        {
            var basket = db.Baskets.ToList().FirstOrDefault(x => x.Id == basketId);
            basket.NumberOfProduct = inputValue;
            db.SaveChanges();
        }

        public void DeleteAll(string userEmail)
        {
            db.Baskets.RemoveRange(Get(userEmail));
            db.SaveChanges();
        }
    }
}
