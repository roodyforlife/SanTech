using SanTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanTech.Interfaces
{
    public interface IDbBasketService
    {
        public void Add(Basket basket);
        public bool AddProductToBasket(string login, int productId);
        public Basket GetByProductIdAndUserId(int productId, int userId);
        public void DeleteFromBasket(int basketId);
        public Basket Get(int basketId);
        public IEnumerable<Basket> GetByUserLogin(string userLogin);
        public void ChangeNumberOfBasket(int basketId, int inputValue);
        public void DeleteAllBasket(string userLogin);
    }
}
