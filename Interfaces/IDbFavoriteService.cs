using SanTech.Models;
using System.Collections.Generic;

namespace SanTech.Interfaces
{
    public interface IDbFavoriteService
    {
        public bool Add(string email, int productId);
        public Favorite Get(int productId, int userId);
        public List<Favorite> Get(string userEmail);
        public void Delete(int favoriteId);
        public Favorite Get(int favoriteId);
        public void DeleteAll(string userEmail);
    }
}
