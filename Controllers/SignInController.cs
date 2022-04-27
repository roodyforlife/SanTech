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
    public class SignInController : Controller
    {
        private readonly IDbUserService dbUserService;
        private readonly IAuthorizatService authorizatService;
        public SignInController(IDbUserService dbUserService, IAuthorizatService authorizatService)
        {
            this.dbUserService = dbUserService;
            this.authorizatService = authorizatService;
        }
        [HttpGet]
        public IActionResult SignIn()
        {
            ViewBag.Account = ControllerContext.HttpContext.Session.GetString("Login");
            return View();
        }
        [HttpPost]
        public IActionResult SignIn(UserLogin user)
        {
            if (!authorizatService.IsRegistered(user.Login))
                ModelState.AddModelError("Login", "Такого аккаунта не существует");
            if(!authorizatService.PasswordIsCorrect(user.Login, user.Password))
                ModelState.AddModelError("Password", "Неверный пароль");
            if (ModelState.IsValid)
            {
                ControllerContext.HttpContext.Session.SetString("Login", user.Login);
                return RedirectPermanent("../Home/Index");            
            }
            return View();
        }
    }
}
