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
            var userEmail = HttpContext.Session.GetString("Email");
            ViewBag.LoggedAccount = userEmail;
            if (userEmail is not null)
            {
                ViewBag.IsAdmin = dbUserService.Get(userEmail).IsAdmin;
                ViewBag.User = dbUserService.Get(userEmail);
            }
            var products = dbProductService.GetProductsInRange(0, 20).ToList();
            var allProducts = dbProductService.GetAll();
            if(allProducts.Count() > 0)
            ViewBag.MaxCost = allProducts.Max(x => x.Cost * (100 - x.SaleProcent) / 100);
            return View(products);
        }
        public string SignOutAccount()
        {
            ControllerContext.HttpContext.Session.Remove("Email");
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
            var userEmail = HttpContext.Session.GetString("Email");
            return dbBasketService.AddProductToBasket(userEmail, Id);
        }
        public void DeleteFromBasket(int basketId)
        {
            dbBasketService.DeleteFromBasket(basketId);
        }
        public ViewResult LoadBasket()
        {
            var userEmail = HttpContext.Session.GetString("Email");
            ViewBag.User = userEmail;
            var model = dbBasketService.GetByUserEmail(userEmail);
            ViewBag.TotalCost = model.Sum(x => x.NumberOfProduct * (x.Product.Cost * (100 - x.Product.SaleProcent) / 100));
            return View(model);
        }
        public void ChangeNumberOfBasket(int basketId, int inputValue)
        {
            dbBasketService.ChangeNumberOfBasket(basketId, inputValue);
        }
        public void DeleteAllBasket()
        {
            var userEmail = HttpContext.Session.GetString("Email");
            dbBasketService.DeleteAllBasket(userEmail);
        }
        [HttpGet]
        public IActionResult RedactProfile()
        {
            var userEmail = HttpContext.Session.GetString("Email");
            ViewBag.LoggedAccount = userEmail;
            if (userEmail is not null)
            {
                ViewBag.IsAdmin = dbUserService.Get(userEmail).IsAdmin;
                ViewBag.User = dbUserService.Get(userEmail);
                return View();
            }
            else
                return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult RedactProfile(User user, IFormFile UploadedFile)
        {
            var userEmail = HttpContext.Session.GetString("Email");
            dbUserService.RedactUser(user, userEmail, UploadedFile);
            return RedirectToAction("Index");
        }
    }
}
