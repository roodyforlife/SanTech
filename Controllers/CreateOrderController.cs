using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SanTech.Interfaces;
using SanTech.Models;

namespace SanTech.Controllers
{
    public class CreateOrderController : Controller
    {
        private readonly IDbUserService _dbUserService;
        private readonly IDbBasketService _dbBasketService;
        private readonly IEmailService _emailService;
        private readonly IOrderService _orderService;
        public CreateOrderController(IDbUserService dbUserService, IDbBasketService dbBasketService, IEmailService emailService, IOrderService orderService)
        {
            _dbUserService = dbUserService;
            _dbBasketService = dbBasketService;
            _emailService = emailService;
            _orderService = orderService;
        }

        [HttpGet]
        public IActionResult CreateOrder()
        {
            var userEmail = HttpContext.Session.GetString("Email");
            if (userEmail is not null)
            {
                if (_dbUserService.Get(userEmail).Basket.Count() != 0)
                {
                    ViewBag.BasketCost = _dbBasketService.Get(userEmail).Sum(x => x.NumberOfProduct * (x.Product.Cost * (100 - x.Product.SaleProcent) / 100));
                    ViewBag.User = _dbUserService.Get(userEmail);
                    return View();
                }
            }

            return Redirect("../Home/Index");
        }

        [HttpPost]
        public IActionResult CreateOrder(OrderViewModel order)
        {
            var userEmail = HttpContext.Session.GetString("Email");
            ViewBag.User = _dbUserService.Get(userEmail);
            ViewBag.BasketCost = _dbBasketService.Get(userEmail).Sum(x => x.NumberOfProduct * (x.Product.Cost * (100 - x.Product.SaleProcent) / 100));
            if (ModelState.IsValid)
            {
                order.Basket = _dbBasketService.Get(userEmail);
                order.User = _dbUserService.Get(userEmail);
                order.TotalCost = order.Basket.Sum(x => x.NumberOfProduct * (x.Product.Cost * (100 - x.Product.SaleProcent) / 100));
                _orderService.Add(order);
                _emailService.SendCheckToEmail(order);
                _dbBasketService.DeleteAll(userEmail);
                return View("../CreateOrder/Created", order);
            }

            return View();
        }
    }
}
