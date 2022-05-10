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
            var userEmail = HttpContext.Session.GetString("Email");
            if (userEmail is not null)
                return Redirect("../Home/Index");
            return View();
        }
        [HttpPost]
        public IActionResult SignInAccount(UserLogin user)
        {
            if (!authorizatService.PasswordIsCorrect(user.Email, user.Password) || !authorizatService.IsRegistered(user.Email))
                ModelState.AddModelError("Password", "Неверный логин или пароль");
            if (ModelState.IsValid)
            {
                HttpContext.Session.SetString("Email", user.Email);
                return RedirectPermanent("../Home/Index");            
            }
            return View();
        }
    }
}
