using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SanTech.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanTech.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDbUserService dbUserService;
        public HomeController(IDbUserService dbUserService)
        {
            this.dbUserService = dbUserService;
        }
        public IActionResult Index()
        {
            var user = ControllerContext.HttpContext.Session.GetString("Login");
            ViewBag.Account = user;
            if(user is not null)
            ViewBag.IsAdmin = dbUserService.Get(user).IsAdmin;
            return View();
        }
        public string SignOutAccount()
        {
            ControllerContext.HttpContext.Session.Remove("Login");
            return "";
        }
    }
}
