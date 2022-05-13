using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using SanTech.Interfaces;
using SanTech.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SanTech.Services
{
    public class OrderService : IOrderService
    {
        private readonly IDbUserService dbUserService;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly ApplicationContext db;
        public OrderService(IDbUserService dbUserService, IHostingEnvironment hostingEnvironment, ApplicationContext db)
        {
            this.dbUserService = dbUserService;
            this.hostingEnvironment = hostingEnvironment;
            this.db = db;
        }

        public void Add(Order order)
        {
            if (order.WriteOffBonuses)
                order.TotalCost = dbUserService.ClearBonuses(order);
            int BonusCredit = order.Basket.Sum(x => x.Product.BonusNumber);
            string pdfPath = Path.Combine(this.hostingEnvironment.WebRootPath, $"Files/Orders/{order.OrderNumber}.pdf");
            Application application = new Application()
            {
                Name = order.Name,
                SecondName = order.SecondName,
                Phone = order.Phone,
                Email = order.Email,
                FilePath = pdfPath,
                User = order.User,
                BonusCredit = BonusCredit,
                Status = "notConfirmed"
            };
            db.Applications.Add(application);
            db.SaveChanges();
        }

        public List<Application> Get()
        {
            return db.Applications.Include(x => x.User).ToList();
        }

        public void Update(int applicationId, string value)
        {
            var application = db.Applications.ToList().FirstOrDefault(x => x.Id == applicationId);
            application.Status = value;
            if(value == "delivered")
            {
                dbUserService.AddBonuses(application);
                application.BonusCredit = 0;
            }
            db.SaveChanges();
        }
    }
}
