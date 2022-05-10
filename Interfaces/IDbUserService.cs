using Microsoft.AspNetCore.Http;
using SanTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanTech.Interfaces
{
    public interface IDbUserService
    {
        public void Add(User user);
        public User Get(string email);
        public IEnumerable<User> GetAll();
        public int ClearBonuses(Order order);
        public void AddBonuses(Order application);
        public string HashData(string data);
        public string HashData(User user);
        public void ChangePassword(User user, string password);
        public void RedactUser(User newUser, string userEmail, IFormFile UploadedFile);
    }
}
