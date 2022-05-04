
using Microsoft.AspNetCore.Authorization;
using SanTech.Interfaces;
using SanTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanTech.Services
{
    public class AuthorizatService : IAuthorizatService
    {
        private readonly ApplicationContext db;
        private readonly IDbUserService dbUserService;
        public AuthorizatService(ApplicationContext db, IDbUserService dbUserService)
        {
            this.db = db;
            this.dbUserService = dbUserService;
        }
        public bool IsRegistered(string email)
        {
            return dbUserService.Get(email) is not null;
        }

        public bool PasswordIsCorrect(string email, string password)
        {
            if (!IsRegistered(email))
                return false;
            return dbUserService.Get(email).Password == dbUserService.HashData(password);
        }
    }
}
