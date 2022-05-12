using Microsoft.EntityFrameworkCore;
using SanTech.Interfaces;
using SanTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public void Add(Basket basket)
        {
            db.Baskets.Add(basket);
            db.SaveChanges();
        }
        public bool AddProductToBasket(string email, int productId)
        {
            var user = dbUserService.Get(email);
            var product = dbProductService.Get(productId);
            if (dbProductService.Get(productId, user.Id) is null)
            {
                Basket basket = new Basket { ProductId = productId, Product = product, UserId = user.Id, NumberOfProduct = 1 };
                Add(basket);
                return true;
            }
            return false;
        }

        public void DeleteFromBasket(int basketId)
        {
            var basket = Get(basketId);
            db.Baskets.Remove(basket);
            db.SaveChanges();
        }

        public Basket Get(int basketId)
        {
            return db.Baskets.Include(x => x.User).Include(x => x.Product).ToList().FirstOrDefault(x => x.Id == basketId);
        }

        public List<Basket> GetByUserEmail(string userEmail)
        {
            return db.Baskets.Include(x => x.Product).Include(x => x.User).ToList().Where(x => x.User.Email == userEmail).ToList();
        }

        public void ChangeNumberOfBasket(int basketId, int inputValue)
        {
            var basket = db.Baskets.ToList().FirstOrDefault(x => x.Id == basketId);
            basket.NumberOfProduct = inputValue;
            db.SaveChanges();
        }
        public void DeleteAllBasket(string userEmail)
        {
            db.Baskets.RemoveRange(GetByUserEmail(userEmail));
            db.SaveChanges();
        }
    }
}
