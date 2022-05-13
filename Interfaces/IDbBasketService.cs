using System.Collections.Generic;
using SanTech.Models;

namespace SanTech.Interfaces
{
    public interface IDbBasketService
    {
        public bool Add(string email, int productId);
        public void Delete(int basketId);
        public Basket Get(int basketId);
        public List<Basket> Get(string userEmail);
        public Basket Get(int productId, int userId);
        public void ChangeNumberOfBasket(int basketId, int inputValue);
        public void DeleteAll(string userEmail);
    }
}
