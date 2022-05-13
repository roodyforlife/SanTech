using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SanTech.Interfaces;
using SanTech.Models;

namespace SanTech.Controllers
{
    public class RegistAccountController : Controller
    {
        private readonly IAuthorizatService _authorizatService;
        private readonly IDbUserService _dbUserService;
        private readonly IEmailService _emailService;
        private readonly IFileService _fileService;
        public RegistAccountController(IAuthorizatService authorizatService, IDbUserService dbUserService, IEmailService emailService, IFileService fileService)
        {
            _authorizatService = authorizatService;
            _dbUserService = dbUserService;
            _emailService = emailService;
            _fileService = fileService;
        }

        [HttpGet]
        public IActionResult RegistAccount()
        {
            var userEmail = HttpContext.Session.GetString("Email");
            if (userEmail is not null)
            {
                return Redirect("../Home/Index");
            }

            return View();
        }

        [HttpPost]
        public IActionResult RegistAccount(User user)
        {
            if (_authorizatService.IsRegistered(user.Email))
            {
                ModelState.AddModelError("Email", "Аккаунт с такой почтой уже существует");
            }

            if (ModelState.IsValid)
            {
                user.Password = _fileService.HashData(user.Password);
                user.PasswordConfirm = _fileService.HashData(user.PasswordConfirm);
                _dbUserService.Add(user);
                _emailService.SendEmail(user.Email, user.Name, "Вы успешно зарегистрировались на сайте SanTech. Запишите ваш пароль, удачных покупок и хорошего настроения!", "emailSend.html");
                HttpContext.Session.SetString("Email", user.Email);
                return RedirectPermanent("../Home/Index");
            }

            return View();
        }
    }
}
