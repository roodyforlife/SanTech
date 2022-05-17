using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SanTech.Interfaces;
using SanTech.Models;

namespace SanTech.Controllers
{
    public class ForgotPasswordController : Controller
    {
        private readonly IDbUserService _dbUserService;
        private readonly IEmailService _emailService;
        private readonly IFileService _fileService;
        public ForgotPasswordController(IDbUserService dbUserService, IEmailService emailService, IFileService fileService)
        {
            _dbUserService = dbUserService;
            _emailService = emailService;
            _fileService = fileService;
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            var userEmail = HttpContext.Session.GetString("Email");
            if (userEmail is not null)
            {
                return Redirect("../Home/Index");
            }

            ViewBag.DisabledButton = "pointer-events: auto";
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(ForgotPassword model)
        {
            var user = _dbUserService.Get(model.Email);
            if (user is not null)
            {
                var code = _fileService.GenerateCode(user);
                var callbackUrl = Url.Action("ResetPassword", "ForgotPassword", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                _emailService.SendEmail(model.Email, user.Name, $"Вы пытаетесь сбросить пароль. Для сброса пароля перейдите по <a href='{callbackUrl}'>ссылке</a>", "emailSend.html");
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
                var user = _dbUserService.Get(model.Email);
                if (user is not null && model.Code == _fileService.GenerateCode(user))
                {
                    model.Password = _fileService.HashData(model.Password);
                    _dbUserService.ChangePassword(user, model.Password);
                    return View("ResetPasswordConfirmation");
                }
            }

            ModelState.AddModelError("Email", "Неверно указана почта");
            return View(model);
        }
    }
}
