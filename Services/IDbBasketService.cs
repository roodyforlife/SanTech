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
        public bool AddProductToBasket(string login, int productId)
        {
            var user = dbUserService.Get(login);
            var product = dbProductService.Get(productId);
            if (GetByProductIdAndUserId(productId, user.Id) is null)
            {
                Basket basket = new Basket { ProductId = productId, Product = product, UserId = user.Id, NumberOfProduct = 1};
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

        public Basket GetByProductIdAndUserId(int productId, int userId)
        {
            return db.Baskets.Include(x => x.Product).Include(x => x.User).ToList().FirstOrDefault(x => x.Product.Id == productId && x.User.Id == userId);
        }

        public List<Basket> GetByUserLogin(string userLogin)
        {
            return db.Baskets.Include(x => x.Product).Include(x => x.User).ToList().Where(x => x.User.Login == userLogin).ToList();
        }

        public void ChangeNumberOfBasket(int basketId, int inputValue)
        {
            var basket = db.Baskets.ToList().FirstOrDefault(x => x.Id == basketId);
            basket.NumberOfProduct = inputValue;
            db.SaveChanges();
        }
        public void DeleteAllBasket(string userLogin)
        {
            db.Baskets.RemoveRange(GetByUserLogin(userLogin));
            db.SaveChanges();
        }

       /* public BasketHistory GetBasketHistory(Basket basket, int applicationId)
        {
            return new BasketHistory()
            {
                ProductId = basket.ProductId,
                UserId = basket.UserId,
                NumberOfProduct = basket.NumberOfProduct,
                ApplicationId = applicationId
            };
        }

        public void SaveBasketHistory(List<Basket> basketsHistory, int applicationId)
        {
            List<BasketHistory> basketHistory = new List<BasketHistory>();
            foreach(var basket in basketsHistory)
            {
                GetBasketHistory(basket, applicationId));
            }
            db.SaveChanges();
        }*/
    }
}
