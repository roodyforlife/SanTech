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
        public HomeController(IDbUserService dbUserService, IDbProductService dbProductService)
        {
            this.dbUserService = dbUserService;
            this.dbProductService = dbProductService;
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
            return ControllerContext.HttpContext.Session.GetString("Login") is null ? false : true;
        }
    }
}
