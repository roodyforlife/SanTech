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
    public class SignInAccountController : Controller
    {
        private readonly IDbUserService dbUserService;
        private readonly IAuthorizatService authorizatService;
        public SignInAccountController(IDbUserService dbUserService, IAuthorizatService authorizatService)
        {
            this.dbUserService = dbUserService;
            this.authorizatService = authorizatService;
        }
        [HttpGet]
        public IActionResult SignInAccount()
        {
            var user = ControllerContext.HttpContext.Session.GetString("Login");
            ViewBag.LoggedAccount = user;
            if (user is not null)
                return Redirect("../Home/Index");
            return View();
        }
        [HttpPost]
        public IActionResult SignInAccount(UserLogin user)
        {
            if (!authorizatService.PasswordIsCorrect(user.Login, user.Password) || !authorizatService.IsRegistered(user.Login))
                ModelState.AddModelError("Password", "Неверный логин или пароль");
            if (ModelState.IsValid)
            {
                ControllerContext.HttpContext.Session.SetString("Login", user.Login);
                return RedirectPermanent("../Home/Index");            
            }
            return View();
        }
    }
}
