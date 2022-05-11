﻿
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
        private readonly IFileService fileService;
        public AuthorizatService(ApplicationContext db, IDbUserService dbUserService, IFileService fileService)
        {
            this.db = db;
            this.dbUserService = dbUserService;
            this.fileService = fileService;
        }
        public bool IsRegistered(string email)
        {
            return dbUserService.Get(email) is not null;
        }

        public bool PasswordIsCorrect(string email, string password)
        {
            if (!IsRegistered(email))
                return false;
            return dbUserService.Get(email).Password == fileService.HashData(password);
        }
    }
}
