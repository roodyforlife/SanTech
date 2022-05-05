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
            var userEmail = HttpContext.Session.GetString("Email");
            if (userEmail is not null)
            {
                if (dbUserService.Get(userEmail).Basket.Count() != 0)
                {
                    ViewBag.BasketCost = dbBasketService.GetByUserEmail(userEmail).Sum(x => x.NumberOfProduct * (x.Product.Cost * (100 - x.Product.SaleProcent) / 100));
                    ViewBag.User = dbUserService.Get(userEmail);
                    return View();
                }
            }
            return Redirect("../Home/Index");
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder(Application application)
        {
            var userEmail = HttpContext.Session.GetString("Email");
            if (ModelState.IsValid)
            {
                application.Basket = dbBasketService.GetByUserEmail(userEmail);
                application.User = dbUserService.Get(userEmail);
                application.TotalCost = application.Basket.Sum(x => x.NumberOfProduct * (x.Product.Cost * (100 - x.Product.SaleProcent) / 100));
                if (application.WriteOffBonuses)
                {
                    application.TotalCost = dbUserService.ClearBonuses(application);
                }
                emailService.SendCheckToEmail(application);
                //dbApplicationService.Add(application);
                dbUserService.AddBonuses(application);
                dbBasketService.DeleteAllBasket(userEmail);
                return View("../CreateOrder/Created", application);
            }
            return View();
        }
        [HttpPost]
        public IActionResult Created()
        {
            var userEmail = HttpContext.Session.GetString("Email");
            ViewBag.LoggedAccount = userEmail;
            if (userEmail is not null)
            {
                ViewBag.IsAdmin = dbUserService.Get(userEmail).IsAdmin;
                ViewBag.User = dbUserService.Get(userEmail);
                //ViewBag.Application = application;
            }
            return View();
        }
    }
}
