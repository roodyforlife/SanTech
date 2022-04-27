﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanTech.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Account = ControllerContext.HttpContext.Session.GetString("Login");
            return View();
        }
        public void SignOutAccount()
        {
            ControllerContext.HttpContext.Session.Remove("Login");
        }
    }
}
