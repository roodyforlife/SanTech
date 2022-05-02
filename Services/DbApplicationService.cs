using SanTech.Interfaces;
using SanTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanTech.Services
{
    public class DbApplicationService : IDbApplicationService
    {
        private readonly ApplicationContext db;
        public DbApplicationService(ApplicationContext db)
        {
            this.db = db;
        }
        public void Add(Application application)
        {
            db.Applications.Add(application);
            db.SaveChanges();
        }
    }
}
