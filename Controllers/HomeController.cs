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
    public class HomeController : Controller
    {
        private readonly IDbUserService dbUserService;
        private readonly IDbProductService dbProductService;
        private readonly IDbBasketService dbBasketService;
        public HomeController(IDbUserService dbUserService, IDbProductService dbProductService, IDbBasketService dbBasketService)
        {
            this.dbUserService = dbUserService;
            this.dbProductService = dbProductService;
            this.dbBasketService = dbBasketService;
        }
        public IActionResult Index()
        {
            //dbProductService.AddThereAre();
            var user = ControllerContext.HttpContext.Session.GetString("Login");
            ViewBag.LoggedAccount = user;
            if (user is not null)
            {
                ViewBag.IsAdmin = dbUserService.Get(user).IsAdmin;
                ViewBag.User = dbUserService.Get(user);
            }
            var products = dbProductService.GetProductsInRange(0, 20).ToList();
            var allProducts = dbProductService.GetAll();
            if(allProducts.Count() > 0)
            ViewBag.MaxCost = allProducts.Max(x => x.Cost * (100 - x.SaleProcent) / 100);
            return View(products);
        }
        public string SignOutAccount()
        {
            ControllerContext.HttpContext.Session.Remove("Login");
            return "<li><a href='../SignInAccount/SignInAccount'><div class='text1 pull__menu__list__text login__button'>Вход/Регистрация</div></a></li>";
        }
        [HttpPost]
        public ViewResult GetAdditionalProducts(int from, int count)
        {
            var products = dbProductService.GetProductsInRange(from, count).ToList();
            return View(products);
        }
        public bool AddToBasket(int Id)
        {
            var user = ControllerContext.HttpContext.Session.GetString("Login");
            return dbBasketService.AddProductToBasket(user, Id);
        }
        public void DeleteFromBasket(int basketId)
        {
            dbBasketService.DeleteFromBasket(basketId);
        }
        public ViewResult LoadBasket()
        {
            var user = ControllerContext.HttpContext.Session.GetString("Login");
            ViewBag.User = user;
            var model = dbBasketService.GetByUserLogin(user);
            ViewBag.TotalCost = model.Sum(x => x.NumberOfProduct * (x.Product.Cost * (100 - x.Product.SaleProcent)) / 100);
            return View(model);
        }
        public void ChangeNumberOfBasket(int basketId, int inputValue)
        {
            dbBasketService.ChangeNumberOfBasket(basketId, inputValue);
        }
        public void DeleteAllBasket()
        {
            var user = ControllerContext.HttpContext.Session.GetString("Login");
            dbBasketService.DeleteAllBasket(user);
        }
    }
}
