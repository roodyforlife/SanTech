using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SanTech.Interfaces;
using SanTech.Models;

namespace SanTech.Services
{
    public class DbFavoriteService : IDbFavoriteService
    {
        private readonly DataBaseContext _db;
        private readonly IDbUserService _dbUserService;
        private readonly IDbProductService _dbProductService;
        public DbFavoriteService(IDbUserService dbUserService, IDbProductService dbProductService, DataBaseContext db)
        {
            _dbUserService = dbUserService;
            _dbProductService = dbProductService;
            _db = db;
        }

        public bool Add(string email, int productId)
        {
            var user = _dbUserService.Get(email);
            var product = _dbProductService.Get(productId);
            if (Get(productId, user.Id) is null)
            {
                Favorite favorite = new Favorite { Product = product, User = user };
                _db.Favorites.Add(favorite);
                _db.SaveChanges();
                return true;
            }

            return false;
        }

        public void Delete(int favoriteId)
        {
            var favorite = Get(favoriteId);
            _db.Favorites.Remove(favorite);
            _db.SaveChanges();
        }

        public void DeleteAll(string userEmail)
        {
            _db.Favorites.RemoveRange(Get(userEmail));
            _db.SaveChanges();
        }

        public Favorite Get(int favoriteId)
        {
            return _db.Favorites.Include(x => x.User).Include(x => x.Product).ToList().FirstOrDefault(x => x.Id == favoriteId);
        }

        public Favorite Get(int productId, int userId)
        {
            return _db.Favorites.Include(x => x.Product).Include(x => x.User).FirstOrDefault(x => x.User.Id == userId && x.Product.Id == productId);
        }

        public List<Favorite> Get(string userEmail)
        {
            return _db.Favorites.Include(x => x.Product).Include(x => x.User).Where(x => x.User.Email == userEmail).ToList();
        }
    }
}
