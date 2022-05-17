using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using SanTech.Interfaces;
using SanTech.Models;

namespace SanTech.Services
{
    public class OrderService : IOrderService
    {
        private readonly IDbUserService _dbUserService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly DataBaseContext _db;
        public OrderService(IDbUserService dbUserService, IHostingEnvironment hostingEnvironment, DataBaseContext db)
        {
            _dbUserService = dbUserService;
            _hostingEnvironment = hostingEnvironment;
            _db = db;
        }

        public void Add(OrderViewModel order)
        {
            if (order.WriteOffBonuses)
            {
                order.TotalCost = _dbUserService.ClearBonuses(order);
            }

            int bonusCredit = order.Basket.Sum(x => x.Product.BonusNumber);
            string pdfPath = Path.Combine(_hostingEnvironment.WebRootPath, $"Files/Orders/{order.OrderNumber}.pdf");
            Application application = new Application()
            {
                Name = order.Name,
                SecondName = order.SecondName,
                Phone = order.Phone,
                Email = order.Email,
                FilePath = pdfPath,
                User = order.User,
                BonusCredit = bonusCredit,
                Status = "notConfirmed"
            };
            _db.Applications.Add(application);
            _db.SaveChanges();
        }

        public List<Application> Get()
        {
            return _db.Applications.Include(x => x.User).ToList();
        }

        public void Update(int applicationId, string value)
        {
            var application = _db.Applications.ToList().FirstOrDefault(x => x.Id == applicationId);
            application.Status = value;
            if (value == "delivered")
            {
                _dbUserService.AddBonuses(application);
                application.BonusCredit = 0;
            }

            _db.SaveChanges();
        }
    }
}
