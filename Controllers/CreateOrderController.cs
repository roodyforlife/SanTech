using IronPdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SanTech.Interfaces;
using SanTech.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SanTech.Controllers
{
    public class CreateOrderController : Controller
    {
        private readonly IDbUserService dbUserService;
        private readonly IDbBasketService dbBasketService;
        private readonly IEmailService emailService;
        public CreateOrderController(IDbUserService dbUserService, IDbBasketService dbBasketService, IEmailService emailService)
        {
            this.dbUserService = dbUserService;
            this.dbBasketService = dbBasketService;
            this.emailService = emailService;
        }
        [HttpGet]
        public IActionResult CreateOrder()
        {
            var user = HttpContext.Session.GetString("Login");
            ViewBag.BasketCost = dbBasketService.GetByUserLogin(user).Sum(x => (x.Product.Cost * x.NumberOfProduct * (100 - x.Product.SaleProcent) / 100));
            ViewBag.User = dbUserService.Get(user);
            return View();
        }
        [HttpPost]
        public IActionResult CreateOrder(Application application)
        {
            var userName = HttpContext.Session.GetString("Login");
            if (ModelState.IsValid)
            {
                application.Basket = dbBasketService.GetByUserLogin(userName);
                application.User = dbUserService.Get(userName);
                int TotalCost = application.Basket.Sum(x => x.NumberOfProduct * (x.Product.Cost * (100 - x.Product.SaleProcent) / 100));
                if (application.WriteOffBonuses)
                {
                   TotalCost -= application.User.Bonus;
                    dbUserService.ClearBonuses(userName);
                }
                application.TotalCost = TotalCost;
                emailService.SendCheckToEmail(application);
                //dbApplicationService.Add(application);
                dbUserService.AddBonuses(application);
                dbBasketService.DeleteAllBasket(userName);
                return View("../CreateOrder/Created", application);
            }
            return View();
        }
        public IActionResult Created()
        {
            var user = ControllerContext.HttpContext.Session.GetString("Login");
            ViewBag.LoggedAccount = user;
            if (user is not null)
            {
                ViewBag.IsAdmin = dbUserService.Get(user).IsAdmin;
                ViewBag.User = dbUserService.Get(user);
                //ViewBag.Application = application;
            }
            return View();
        }
    }
}
