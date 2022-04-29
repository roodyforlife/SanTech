using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SanTech.Interfaces;
using SanTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanTech.Controllers
{
    public class AdminPanelController : Controller
    {
        private readonly IDbProductService dbProductService;
        private readonly IFileService fileService;
        private readonly IDbUserService dbUserService;
        public AdminPanelController(IDbProductService dbProductService, IFileService fileService, IDbUserService dbUserService)
        {
            this.dbProductService = dbProductService;
            this.fileService = fileService;
            this.dbUserService = dbUserService;
        }
        public IActionResult AdminPanel()
        {
            var user = ControllerContext.HttpContext.Session.GetString("Login");
            if(user is null || !dbUserService.Get(user).IsAdmin)
                return Redirect("../Home/Index");
            var isAdmin = dbUserService.Get(user).IsAdmin;
                ViewBag.LoggedAccount = user;
                ViewBag.IsAdmin = isAdmin;
                ViewBag.User = dbUserService.Get(user);
            return View();
        }
        [HttpPost]
        public bool CreateNewProduct(Product product, IFormFile UploadedFile)
        {
            if (product.Title is null || product.Desc is null || product.Cost == 0 || UploadedFile is null)
                return true;
            product.Image = fileService.FromImageToByte(UploadedFile);
            dbProductService.Add(product);
            return false;
        }
    }
}
