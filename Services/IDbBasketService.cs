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
        public DbBasketService(ApplicationContext db, IDbUserService dbUserService)
        {
            this.db = db;
            this.dbUserService = dbUserService;
        }
        public void Add(Basket basket)
        {
            db.Baskets.Add(basket);
            db.SaveChanges();
        }
        public bool AddProductToBasket(string login, int productId)
        {
            var user = dbUserService.Get(login);
            if (GetByProductIdAndUserId(productId, user.Id) is null)
            {
                Basket basket = new Basket { ProductId = productId, UserId = user.Id, NumberOfProduct = 1};
                Add(basket);
                return true;
            }
            return false;
        }

        public void DeleteFromBasket(int basketId)
        {
            var basket = GetWithName(basketId);
            db.Baskets.Remove(basket);
            db.SaveChanges();
        }

        public Basket GetWithName(int basketId)
        {
            return db.Baskets.Include(x => x.User).Include(x => x.Product).ToList().FirstOrDefault(x => x.Id == basketId);
        }

        public Basket GetByProductIdAndUserId(int productId, int userId)
        {
            return db.Baskets.Include(x => x.Product).Include(x => x.User).ToList().FirstOrDefault(x => x.Product.Id == productId && x.User.Id == userId);
        }

        public IEnumerable<Basket> GetByUserLogin(string userLogin)
        {
            return db.Baskets.Include(x => x.Product).Include(x => x.User).ToList().Where(x => x.User.Login == userLogin);
        }

        public void ChangeNumberOfBasket(int basketId, string userLogin, int inputValue)
        {
            var basket = db.Baskets.ToList().FirstOrDefault(x => x.Id == basketId);
            basket.NumberOfProduct = inputValue;
            db.SaveChanges();
        }
    }
}
