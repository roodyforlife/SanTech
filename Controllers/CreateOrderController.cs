using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SanTech.Interfaces;
using SanTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanTech.Controllers
{
    public class CreateOrderController : Controller
    {
        private readonly IDbUserService dbUserService;
        private readonly IDbBasketService dbBasketService;
        private readonly IDbApplicationService dbApplicationService;
        public CreateOrderController(IDbUserService dbUserService, IDbBasketService dbBasketService, IDbApplicationService dbApplicationService)
        {
            this.dbUserService = dbUserService;
            this.dbBasketService = dbBasketService;
            this.dbApplicationService = dbApplicationService;
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
            var user = HttpContext.Session.GetString("Login");
            application.Basket.AddRange(dbBasketService.GetByUserLogin(user));
            if(ModelState.IsValid)
            {
                dbApplicationService.Add(application);
                dbBasketService.DeleteAllBasket(user);
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
