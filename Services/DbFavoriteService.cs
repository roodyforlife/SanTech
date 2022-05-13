using Microsoft.EntityFrameworkCore;
using SanTech.Interfaces;
using SanTech.Models;
using System.Collections.Generic;
using System.Linq;

namespace SanTech.Services
{
    public class DbFavoriteService : IDbFavoriteService
    {
        private readonly ApplicationContext db;
        private readonly IDbUserService dbUserService;
        private readonly IDbProductService dbProductService;
        public DbFavoriteService(IDbUserService dbUserService, IDbProductService dbProductService, ApplicationContext db)
        {
            this.dbUserService = dbUserService;
            this.dbProductService = dbProductService;
            this.db = db;
        }

        public bool Add(string email, int productId)
        {
            var user = dbUserService.Get(email);
            var product = dbProductService.Get(productId);
            if (Get(productId, user.Id) is null)
            {
                Favorite favorite = new Favorite { Product = product, User = user};
                db.Favorites.Add(favorite);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public void Delete(int favoriteId)
        {
            var favorite = Get(favoriteId);
            db.Favorites.Remove(favorite);
            db.SaveChanges();
        }

        public void DeleteAll(string userEmail)
        {
            db.Favorites.RemoveRange(Get(userEmail));
            db.SaveChanges();
        }

        public Favorite Get(int favoriteId)
        {
            return db.Favorites.Include(x => x.User).Include(x => x.Product).ToList().FirstOrDefault(x => x.Id == favoriteId);
        }

        public Favorite Get(int productId, int userId)
        {
            return db.Favorites.Include(x => x.Product).Include(x => x.User).FirstOrDefault(x => x.User.Id == userId && x.Product.Id == productId);
        }

        public List<Favorite> Get(string userEmail)
        {
            return db.Favorites.Include(x => x.Product).Include(x => x.User).Where(x => x.User.Email == userEmail).ToList();
        }
    }
}
