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
            var userEmail = HttpContext.Session.GetString("Email");
            if(userEmail is null || !dbUserService.Get(userEmail).IsAdmin)
                return Redirect("../Home/Index");
            var isAdmin = dbUserService.Get(userEmail).IsAdmin;
                ViewBag.LoggedAccount = userEmail;
                ViewBag.IsAdmin = isAdmin;
                ViewBag.User = dbUserService.Get(userEmail);
                ViewBag.UserBase = dbUserService.GetAll();
               // ViewBag.ApplicationBase = dbApplicationService.GetAll();
            return View(dbProductService.GetProductsInRange(0, 20).ToList());
        }
        public bool AddNewProduct(CreateProduct product)
        {
            if (product.Title is null || product.Desc is null || product.Cost == 0 || product.UploadedFile is null || product.SaleProcent < 0 || product.SaleProcent > 100)
                return true;
            dbProductService.Add(product);
            return false;
        }
        [HttpPost]
        public ViewResult GetAdditionalProducts(int from, int count)
        {
            var products = dbProductService.GetProductsInRange(from, count).ToList();
            return View(products);
        }
    }
}
