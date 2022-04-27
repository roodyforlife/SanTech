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
    public class RegistratController : Controller
    {
        private readonly IAuthorizatService authorizatService;
        private readonly IDbUserService dbUserService;
        public RegistratController(IAuthorizatService authorizatService, IDbUserService dbUserService)
        {
            this.authorizatService = authorizatService;
            this.dbUserService = dbUserService;
        }
        [HttpGet]
        public IActionResult Registrat()
        {
            ViewBag.Account = ControllerContext.HttpContext.Session.GetString("Login");
            return View();
        }
        [HttpPost]
        public IActionResult Registrat(User user)
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
