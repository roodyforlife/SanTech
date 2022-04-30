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
                Basket basket = new Basket { ProductId = productId, UserId = user.Id };
                Add(basket);
                return true;
            }
            return false;
        }

        public void DeleteFromBasket(int basketId, string userLogin)
        {
            var basket = GetWithName(basketId, userLogin);
            db.Baskets.Remove(basket);
            db.SaveChanges();
        }

        public Basket GetWithName(int basketId, string userLogin)
        {
            return db.Baskets.Include(x => x.User).ToList().Where(x => x.User.Login == userLogin).FirstOrDefault(x => x.Id == basketId);
        }

        public Basket GetByProductIdAndUserId(int productId, int userId)
        {
            return db.Baskets.Include(x => x.Product).ToList().FirstOrDefault(x => x.Product.Id == productId && x.User.Id == userId);
        }
    }
}
