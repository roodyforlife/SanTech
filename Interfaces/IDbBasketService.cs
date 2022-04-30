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
        public void DeleteFromBasket(int basketId, string userLogin);
        public Basket GetWithName(int basketId, string userLogin);
    }
}
