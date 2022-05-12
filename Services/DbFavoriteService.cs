using SanTech.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanTech.Services
{
    public class DbFavoriteService : IDbFavoriteService
    {
        private readonly IDbUserService dbUserService;
        private readonly IDbProductService dbProductService;
        public DbFavoriteService(IDbUserService dbUserService, IDbProductService dbProductService)
        {
            this.dbUserService = dbUserService;
            this.dbProductService = dbProductService;
        }
        public bool Add(string email, int productId)
        {
            var user = dbUserService.Get(email);
            var product = dbProductService.Get(productId);
            if (dbProductService.Get(productId, user.Id) is null)
            {
                Favourite basket = new Favourite { ProductId = productId, Product = product, UserId = user.Id, NumberOfProduct = 1 };
                Add(basket);
                return true;
            }
            return false;
        }

    }
}
