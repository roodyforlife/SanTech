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
        public bool AddProductToBasket(string email, int productId);
        public void DeleteFromBasket(int basketId);
        public Basket Get(int basketId);
        public List<Basket> GetByUserEmail(string userEmail);
        public void ChangeNumberOfBasket(int basketId, int inputValue);
        public void DeleteAllBasket(string userEmail);
    }
}
