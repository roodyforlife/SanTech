using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SanTech.Interfaces;
using SanTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SanTech.Controllers
{
    public class RegistAccountController : Controller
    {
        private readonly IAuthorizatService authorizatService;
        private readonly IDbUserService dbUserService;
        private readonly IEmailService emailService;
        public RegistAccountController(IAuthorizatService authorizatService, IDbUserService dbUserService, IEmailService emailService)
        {
            this.authorizatService = authorizatService;
            this.dbUserService = dbUserService;
            this.emailService = emailService;
        }
        [HttpGet]
        public IActionResult RegistAccount()
        {
            var user = ControllerContext.HttpContext.Session.GetString("Login");
            ViewBag.LoggedAccount = user;
            if (user is not null)
                return Redirect("../Home/Index");
            return View();
        }
        [HttpPost]
        public IActionResult RegistAccount(User user)
        {
            if (authorizatService.IsRegistered(user.Login))
                ModelState.AddModelError("Login", "Такой аккаунт уже существует");
            if (ModelState.IsValid)
            {
                user.Password = dbUserService.HashData(user.Password);
                user.PasswordConfirm = dbUserService.HashData(user.PasswordConfirm);
                dbUserService.Add(user);
                emailService.SendEmail(user.Email, user.Name, "Вы успешно зарегистрировались на сайте SanTech. Запишите ваш пароль, удачных покупок и хорошего настроения!", "emailSend.html");
                ControllerContext.HttpContext.Session.SetString("Login", user.Login);
                return RedirectPermanent("../Home/Index");
            }
            return View();
        }
    }
}
