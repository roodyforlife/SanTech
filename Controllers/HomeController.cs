using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SanTech.Interfaces;
using SanTech.Models;
using SanTech.Models.ViewModels;

namespace SanTech.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDbUserService _dbUserService;
        private readonly IDbProductService _dbProductService;
        private readonly IDbBasketService _dbBasketService;
        private readonly IDbFavoriteService _dbFavoriteService;
        public HomeController(IDbUserService dbUserService, IDbProductService dbProductService, IDbBasketService dbBasketService, IDbFavoriteService dbFavoriteService)
        {
            _dbUserService = dbUserService;
            _dbProductService = dbProductService;
            _dbBasketService = dbBasketService;
            _dbFavoriteService = dbFavoriteService;
        }

        public IActionResult Index(SearchViewModel search)
        {
            var userEmail = HttpContext.Session.GetString("Email");
            ViewBag.LoggedAccount = userEmail;
            if (userEmail is not null)
            {
                ViewBag.User = _dbUserService.Get(userEmail);
            }

            var allProducts = _dbProductService.GetAll();
            var products = _dbProductService.GetProductsInRange(0, 20, _dbProductService.Get(search)).ToList();
            if (allProducts.Count() > 0)
            {
                ViewBag.MaxCost = _dbProductService.GetAll().Max(x => (x.Cost * (100 - x.SaleProcent) / 100));
            }

            ViewBag.Search = search;
            return View(products);
        }

        public string SignOutAccount()
        {
            ControllerContext.HttpContext.Session.Remove("Email");
            return "<li><a href='../SignInAccount/SignInAccount'><div class='text1 pull__menu__list__text login__button'>Вход/Регистрация</div></a></li>";
        }

        [HttpPost]
        public ViewResult GetAdditionalProducts(int from, int count, SearchViewModel search)
        {
            var allProducts = _dbProductService.Get(search);
            var products = _dbProductService.GetProductsInRange(from, count, allProducts).ToList();
            return View(products);
        }

        public bool AddToBasket(int id)
        {
            var userEmail = HttpContext.Session.GetString("Email");
            return _dbBasketService.Add(userEmail, id);
        }

        public bool AddToFavourites(int id)
        {
            var userEmail = HttpContext.Session.GetString("Email");
            return _dbFavoriteService.Add(userEmail, id);
        }

        public void DeleteFromBasket(int basketId)
        {
            _dbBasketService.Delete(basketId);
        }

        public void DeleteFromFavorites(int favoriteId)
        {
            _dbFavoriteService.Delete(favoriteId);
        }

        public ViewResult LoadBasket()
        {
            var userEmail = HttpContext.Session.GetString("Email");
            ViewBag.User = userEmail;
            var model = _dbBasketService.Get(userEmail);
            ViewBag.TotalCost = model.Sum(x => x.NumberOfProduct * (x.Product.Cost * (100 - x.Product.SaleProcent) / 100));
            return View(model);
        }

        public ViewResult LoadFavorites()
        {
            var userEmail = HttpContext.Session.GetString("Email");
            ViewBag.User = userEmail;
            var model = _dbFavoriteService.Get(userEmail);
            return View(model);
        }

        public void ChangeNumberOfBasket(int basketId, int inputValue)
        {
            _dbBasketService.ChangeNumberOfBasket(basketId, inputValue);
        }

        public void DeleteAllBasket()
        {
            var userEmail = HttpContext.Session.GetString("Email");
            _dbBasketService.DeleteAll(userEmail);
        }

        public void DeleteAllFavorites()
        {
            var userEmail = HttpContext.Session.GetString("Email");
            _dbFavoriteService.DeleteAll(userEmail);
        }

        [HttpGet]
        public IActionResult UpdateUser()
        {
            var userEmail = HttpContext.Session.GetString("Email");
            if (userEmail is not null)
            {
                ViewBag.User = _dbUserService.Get(userEmail);
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult UpdateUser(User user, IFormFile uploadedFile)
        {
            var userEmail = HttpContext.Session.GetString("Email");
            _dbUserService.UpdateUser(user, userEmail, uploadedFile);
            return RedirectToAction("Index");
        }
    }
}
