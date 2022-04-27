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
    public class RegistAccountController : Controller
    {
        private readonly IAuthorizatService authorizatService;
        private readonly IDbUserService dbUserService;
        public RegistAccountController(IAuthorizatService authorizatService, IDbUserService dbUserService)
        {
            this.authorizatService = authorizatService;
            this.dbUserService = dbUserService;
        }
        [HttpGet]
        public IActionResult RegistAccount()
        {
            var user = ControllerContext.HttpContext.Session.GetString("Login");
            ViewBag.Account = user;
            if (user is not null)
                return RedirectPermanent("../Home/Index");
            return View();
        }
        [HttpPost]
        public IActionResult RegistAccount(User user)
        {
            if(authorizatService.IsRegistered(user.Login))
            {
                ModelState.AddModelError("Login", "Такой аккаунт уже существует");
            }
            if (ModelState.IsValid)
            {
                dbUserService.Add(user);
                ControllerContext.HttpContext.Session.SetString("Login", user.Login);
                return RedirectPermanent("../Home/Index");
            }
            return View();
        }
    }
}
