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

        public User Get(string user)
        {
            return db.Users.ToList().FirstOrDefault(x => x.Login == user);
        }
    }
}
