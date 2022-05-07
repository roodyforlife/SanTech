using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SanTech.Interfaces;
using SanTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanTech.Controllers
{
    public class ForgotPasswordController : Controller
    {
        private readonly IDbUserService dbUserService;
        private readonly IEmailService emailService;
        public ForgotPasswordController(IDbUserService dbUserService, IEmailService emailService)
        {
            this.dbUserService = dbUserService;
            this.emailService = emailService;
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            var userEmail = HttpContext.Session.GetString("Email");
            if (userEmail is not null)
                return Redirect("../Home/Index");
            ViewBag.DisabledButton = "pointer-events: auto";
            return View();
        }
        [HttpPost]
        public IActionResult ForgotPassword(ForgotPassword model)
        {
            var user = dbUserService.Get(model.Email);
            if(user is not null)
            {
                var code = dbUserService.HashData(user);
                var callbackUrl = Url.Action("ResetPassword", "ForgotPassword", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                emailService.SendEmail(model.Email, user.Name, $"Вы пытаетесь сбросить пароль. Для сброса пароля перейдите по <a href='{callbackUrl}'>ссылке</a>", "emailSend.html");
                return View("ForgotPasswordConfirmation");
            }
            ModelState.AddModelError("Email", "Аккаунта с такой почтой не существует");
            ViewBag.DisabledButton = "pointer-events: auto";
            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? Redirect("../Home/Index") : View();
        }
        [HttpPost]
        public IActionResult ResetPassword(ResetPassword model)
        {
            if (ModelState.IsValid)
            {
                var user = dbUserService.Get(model.Email);
                if (user is not null && model.Code == dbUserService.HashData(user))
                {
                    model.Password = dbUserService.HashData(model.Password);
                    dbUserService.ChangePassword(user, model.Password);
                    return View("ResetPasswordConfirmation");
                }
            }
            ModelState.AddModelError("Email", "Неверно указана почта");
            return View(model);
        }
    }
}
