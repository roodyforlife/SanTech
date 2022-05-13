using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SanTech.Interfaces;
using SanTech.Models;

namespace SanTech.Controllers
{
    public class SignInAccountController : Controller
    {
        private readonly IAuthorizatService _authorizatService;
        public SignInAccountController(IAuthorizatService authorizatService)
        {
            _authorizatService = authorizatService;
        }

        [HttpGet]
        public IActionResult SignInAccount()
        {
            var userEmail = HttpContext.Session.GetString("Email");
            if (userEmail is not null)
            {
                return Redirect("../Home/Index");
            }

            return View();
        }

        [HttpPost]
        public IActionResult SignInAccount(UserLogin user)
        {
            if (!_authorizatService.PasswordIsCorrect(user.Email, user.Password) || !_authorizatService.IsRegistered(user.Email))
            {
                ModelState.AddModelError("Password", "Неверный логин или пароль");
            }

            if (ModelState.IsValid)
            {
                HttpContext.Session.SetString("Email", user.Email);
                return RedirectPermanent("../Home/Index");
            }

            return View();
        }
    }
}
