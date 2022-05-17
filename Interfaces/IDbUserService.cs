using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using SanTech.Models;

namespace SanTech.Interfaces
{
    public interface IDbUserService
    {
        public void Add(User user);
        public User Get(string email);
        public IEnumerable<User> GetAll();
        public int ClearBonuses(OrderViewModel order);
        public void AddBonuses(Application application);
        public void ChangePassword(User user, string password);
        public void UpdateUser(User newUser, string userEmail, IFormFile uploadedFile);
    }
}
