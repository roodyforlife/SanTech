using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SanTech.Interfaces;
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
        public CreateOrderController(IDbUserService dbUserService, IDbBasketService dbBasketService)
        {
            this.dbUserService = dbUserService;
            this.dbBasketService = dbBasketService;
        }
        public IActionResult CreateOrder()
        {
            var user = ControllerContext.HttpContext.Session.GetString("Login");
            ViewBag.Basket = dbBasketService.GetByUserLogin(user).Sum(x => (x.Product.Cost * x.NumberOfProduct * (100 - x.Product.SaleProcent) / 100));
            ViewBag.User = dbUserService.Get(user);
            return View();
        }
    }
}
