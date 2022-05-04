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
            return View();
        }
        [HttpPost]
        public IActionResult ForgotPassword(ForgotPassword model)
        {
            var user = dbUserService.GetUserByEmail(model.Email);
            if(user is not null)
            {
                var code = dbUserService.HashTokenFromUser(user);
                var callbackUrl = Url.Action("ResetPassword", "ForgotPassword", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                emailService.SendEmail(model.Email, user.Name, $"Для сброса пароля перейдите по ссылке: <a href='{callbackUrl}'>link</a>", "emailSend.html");
                return View("ForgotPasswordConfirmation");
            }
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
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = dbUserService.GetUserByEmail(model.Email);
            if (user is not null && model.Code == dbUserService.HashTokenFromUser(user))
            {
                model.Password = dbUserService.HashData(model.Password);
                dbUserService.ChangePassword(user, model.Password);
            }
               return Redirect("../Home/Index");

        }
    }
}
