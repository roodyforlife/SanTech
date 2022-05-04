﻿using SanTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanTech.Interfaces
{
    public interface IDbUserService
    {
        public void Add(User user);
        public User Get(string user);
        public IEnumerable<User> GetAll();
        public int ClearBonuses(Application application);
        public void AddBonuses(Application application);
        public string HashData(string data);
        public string HashTokenFromUser(User user);
        public void ChangePassword(User user, string password);
        public User GetUserByEmail(string email);
        //public string GeneratePasswordResetToken();
    }
}
