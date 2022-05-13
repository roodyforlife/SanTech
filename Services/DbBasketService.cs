using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SanTech.Interfaces;
using SanTech.Models;

namespace SanTech.Services
{
    public class DbBasketService : IDbBasketService
    {
        private readonly ApplicationContext _db;
        private readonly IDbUserService _dbUserService;
        private readonly IDbProductService _dbProductService;
        public DbBasketService(ApplicationContext db, IDbUserService dbUserService, IDbProductService dbProductService)
        {
            _db = db;
            _dbUserService = dbUserService;
            _dbProductService = dbProductService;
        }

        public bool Add(string email, int productId)
        {
            var user = _dbUserService.Get(email);
            var product = _dbProductService.Get(productId);
            if (Get(productId, user.Id) is null)
            {
                Basket basket = new Basket { Product = product, User = user, NumberOfProduct = 1 };
                _db.Baskets.Add(basket);
                _db.SaveChanges();
                return true;
            }

            return false;
        }

        public void Delete(int basketId)
        {
            var basket = Get(basketId);
            _db.Baskets.Remove(basket);
            _db.SaveChanges();
        }

        public Basket Get(int basketId)
        {
            return _db.Baskets.Include(x => x.User).Include(x => x.Product).ToList().FirstOrDefault(x => x.Id == basketId);
        }

        public Basket Get(int productId, int userId)
        {
            return _db.Baskets.Include(x => x.Product).Include(x => x.User).
                ToList().FirstOrDefault(x => x.Product.Id == productId && x.User.Id == userId);
        }

        public List<Basket> Get(string userEmail)
        {
            return _db.Baskets.Include(x => x.Product).Include(x => x.User).ToList().Where(x => x.User.Email == userEmail).ToList();
        }

        public void ChangeNumberOfBasket(int basketId, int inputValue)
        {
            var basket = _db.Baskets.ToList().FirstOrDefault(x => x.Id == basketId);
            basket.NumberOfProduct = inputValue;
            _db.SaveChanges();
        }

        public void DeleteAll(string userEmail)
        {
            _db.Baskets.RemoveRange(Get(userEmail));
            _db.SaveChanges();
        }
    }
}
