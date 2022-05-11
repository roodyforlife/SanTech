using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SanTech.Interfaces;
using SanTech.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanTech.Services
{
    public class DbUserService : IDbUserService
    {
        private readonly ApplicationContext db;
        private readonly IFileService fileService;
        private readonly IHostingEnvironment hostingEnvironment;
        public DbUserService(ApplicationContext db, IFileService fileService, IHostingEnvironment hostingEnvironment)
        {
            this.db = db;
            this.fileService = fileService;
            this.hostingEnvironment = hostingEnvironment;
        }
        public void Add(User user)
        {
            user.Avatar = fileService.FromImageToByte("avatar.png");
            db.Users.Add(user);
            db.SaveChanges();
        }

        public void AddBonuses(Application application)
        {
            var user = db.Users.ToList().FirstOrDefault(x => x.Email == application.User.Email);
            user.Bonus += application.BonusCredit;
            db.SaveChanges();
        }

        public void ChangePassword(User user, string password)
        {
            var newUser = db.Users.ToList().FirstOrDefault(x => x.Email == user.Email);
            newUser.Password = password;
            newUser.PasswordConfirm = password;
            db.SaveChanges();
        }

        public int ClearBonuses(Order order)
        {
            var user = db.Users.ToList().FirstOrDefault(x => x.Email == order.User.Email);
            if(order.TotalCost >= order.User.Bonus)
            {
                order.TotalCost -= order.User.Bonus;
                user.Bonus = 0;
            }
            else
            {
                user.Bonus -= order.TotalCost;
                order.TotalCost = 0;
            }
            db.SaveChanges();
            return order.TotalCost;
        }

        public User Get(string email)
        {
            return db.Users.Include(x => x.Basket).ToList().FirstOrDefault(x => x.Email == email);
        }

        public IEnumerable<User> GetAll()
        {
            return db.Users.ToList();
        }

        public void RedactUser(User newUser, string userEmail, IFormFile UploadedFile)
        {
            var user = db.Users.ToList().FirstOrDefault(x => x.Email == userEmail);
            user.Name = newUser.Name;
            user.Avatar = UploadedFile is null ? user.Avatar : fileService.FromImageToByte(UploadedFile);
            user.Password = newUser.Password is null ? user.Password : fileService.HashData(newUser.Password) ;
            user.PasswordConfirm = newUser.PasswordConfirm is null ? user.PasswordConfirm : fileService.HashData(newUser.PasswordConfirm);
            user.Phone = newUser.Phone;
            db.SaveChanges();
        }
    }
}
