using Microsoft.EntityFrameworkCore;
using SanTech.Interfaces;
using SanTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanTech.Services
{
    public class DbUserService : IDbUserService
    {
        private readonly ApplicationContext db;
        public DbUserService(ApplicationContext db)
        {
            this.db = db;
        }
        public void Add(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
        }

        public void ClearBonuses(string userLogin)
        {
            var user = db.Users.ToList().FirstOrDefault(x => x.Login == userLogin);
            user.Bonus = 0;
            db.SaveChanges();
        }

        public User Get(string user)
        {
            return db.Users.Include(x => x.Basket).ToList().FirstOrDefault(x => x.Login == user);
        }

        public IEnumerable<User> GetAll()
        {
            return db.Users.ToList();
        }
    }
}
