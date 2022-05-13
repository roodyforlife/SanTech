using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SanTech.Interfaces;
using SanTech.Models;

namespace SanTech.Controllers
{
    public class RegistAccountController : Controller
    {
        private readonly IAuthorizatService authorizatService;
        private readonly IDbUserService dbUserService;
        private readonly IEmailService emailService;
        private readonly IFileService fileService;
        public RegistAccountController(IAuthorizatService authorizatService, IDbUserService dbUserService, IEmailService emailService, IFileService fileService)
        {
            this.authorizatService = authorizatService;
            this.dbUserService = dbUserService;
            this.emailService = emailService;
            this.fileService = fileService;
        }

        [HttpGet]
        public IActionResult RegistAccount()
        {
            var userEmail = HttpContext.Session.GetString("Email");
            if (userEmail is not null)
                return Redirect("../Home/Index");
            return View();
        }

        [HttpPost]
        public IActionResult RegistAccount(User user)
        {
            if (authorizatService.IsRegistered(user.Email))
                ModelState.AddModelError("Email", "Аккаунт с такой почтой уже существует");
            if (ModelState.IsValid)
            {
                user.Password = fileService.HashData(user.Password);
                user.PasswordConfirm = fileService.HashData(user.PasswordConfirm);
                dbUserService.Add(user);
                emailService.SendEmail(user.Email, user.Name, "Вы успешно зарегистрировались на сайте SanTech. Запишите ваш пароль, удачных покупок и хорошего настроения!", "emailSend.html");
                HttpContext.Session.SetString("Email", user.Email);
                return RedirectPermanent("../Home/Index");
            }
            return View();
        }

    }
}
