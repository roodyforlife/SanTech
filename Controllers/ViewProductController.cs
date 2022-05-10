﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SanTech.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanTech.Controllers
{
    public class ViewProductController : Controller
    {
        private readonly IDbUserService dbUserService;
        private readonly IDbProductService dbProductService;
        public ViewProductController(IDbUserService dbUserService, IDbProductService dbProductService)
        {
            this.dbUserService = dbUserService;
            this.dbProductService = dbProductService;
        }
        public IActionResult ViewProduct(int productId)
        {
            var userEmail = HttpContext.Session.GetString("Email");
            if (userEmail is not null)
            {
                ViewBag.User = dbUserService.Get(userEmail);
            }
            var model = dbProductService.Get(productId);
            model.Comments = model.Comments.OrderByDescending(x => x.Id).ToList();
            return View(model);
        }
    }
}
