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
        private readonly IOrderService orderService;
        public CreateOrderController(IDbUserService dbUserService, IDbBasketService dbBasketService, IEmailService emailService, IOrderService orderService)
        {
            this.dbUserService = dbUserService;
            this.dbBasketService = dbBasketService;
            this.emailService = emailService;
            this.orderService = orderService;
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
        public async Task<IActionResult> CreateOrder(Order order)
        {
            var userEmail = HttpContext.Session.GetString("Email");
            if (ModelState.IsValid)
            {
                order.Basket = dbBasketService.GetByUserEmail(userEmail);
                order.User = dbUserService.Get(userEmail);
                order.TotalCost = order.Basket.Sum(x => x.NumberOfProduct * (x.Product.Cost * (100 - x.Product.SaleProcent) / 100));
                orderService.Add(order);
                emailService.SendCheckToEmail(order);
                dbBasketService.DeleteAllBasket(userEmail);
                ViewBag.User = dbUserService.Get(userEmail);
                return View("../CreateOrder/Created", order);
            }
            return View();
        }
       
    }
}
