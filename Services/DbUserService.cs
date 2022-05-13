using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SanTech.Interfaces;
using SanTech.Models;

namespace SanTech.Services
{
    public class DbUserService : IDbUserService
    {
        private readonly ApplicationContext _db;
        private readonly IFileService _fileService;
        public DbUserService(ApplicationContext db, IFileService fileService)
        {
            _db = db;
            _fileService = fileService;
        }

        public void Add(User user)
        {
            user.Avatar = _fileService.FromImageToByte("avatar.png");
            _db.Users.Add(user);
            _db.SaveChanges();
        }

        public void AddBonuses(Application application)
        {
            var user = _db.Users.ToList().FirstOrDefault(x => x.Email == application.User.Email);
            user.Bonus += application.BonusCredit;
            _db.SaveChanges();
        }

        public void ChangePassword(User user, string password)
        {
            var newUser = _db.Users.ToList().FirstOrDefault(x => x.Email == user.Email);
            newUser.Password = password;
            newUser.PasswordConfirm = password;
            _db.SaveChanges();
        }

        public int ClearBonuses(Order order)
        {
            var user = _db.Users.ToList().FirstOrDefault(x => x.Email == order.User.Email);
            if (order.TotalCost >= order.User.Bonus)
            {
                order.TotalCost -= order.User.Bonus;
                user.Bonus = 0;
            }
            else
            {
                user.Bonus -= order.TotalCost;
                order.TotalCost = 0;
            }

            _db.SaveChanges();
            return order.TotalCost;
        }

        public User Get(string email)
        {
            return _db.Users.Include(x => x.Basket).Include(x => x.Favorites).ToList().FirstOrDefault(x => x.Email == email);
        }

        public IEnumerable<User> GetAll()
        {
            return _db.Users.ToList();
        }

        public void UpdateUser(User newUser, string userEmail, IFormFile uploadedFile)
        {
            var user = _db.Users.ToList().FirstOrDefault(x => x.Email == userEmail);
            user.Name = newUser.Name;
            user.Avatar = uploadedFile is null ? user.Avatar : _fileService.FromImageToByte(uploadedFile);
            user.Password = newUser.Password is null ? user.Password : _fileService.HashData(newUser.Password);
            user.PasswordConfirm = newUser.PasswordConfirm is null ? user.PasswordConfirm : _fileService.HashData(newUser.PasswordConfirm);
            user.Phone = newUser.Phone;
            _db.SaveChanges();
        }
    }
}
