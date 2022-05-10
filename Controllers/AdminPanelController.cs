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
        private readonly IOrderService orderService;
        public AdminPanelController(IDbProductService dbProductService, IFileService fileService, IDbUserService dbUserService, IOrderService orderService)
        {
            this.dbProductService = dbProductService;
            this.fileService = fileService;
            this.dbUserService = dbUserService;
            this.orderService = orderService;
        }
        public IActionResult AdminPanel()
            {
            var userEmail = HttpContext.Session.GetString("Email");
            if(userEmail is null || !dbUserService.Get(userEmail).IsAdmin)
                return Redirect("../Home/Index");
            var isAdmin = dbUserService.Get(userEmail).IsAdmin;
                //ViewBag.LoggedAccount = userEmail;
                //ViewBag.IsAdmin = isAdmin;
                ViewBag.User = dbUserService.Get(userEmail);
                ViewBag.UserBase = dbUserService.GetAll();
            ViewBag.ApplicationsBase = orderService.Get();
            var allProducts = dbProductService.GetAll();
            return View(dbProductService.GetProductsInRange(0, 20, allProducts).ToList());
        }
        public bool AddNewProduct(ProductViewModel product)
        {
            if (product.Title is null || product.Desc is null || product.Cost == 0 || product.UploadedFile is null || product.SaleProcent < 0 || product.SaleProcent > 100 || product.CategoryId == 0)
                return true;
            dbProductService.Add(product);
            return false;
        }
        [HttpPost]
        public ViewResult GetAdditionalProducts(int from, int count)
        {
            var allProducts = dbProductService.GetAll();
            var products = dbProductService.GetProductsInRange(from, count, allProducts).ToList();
            return View(products);
        }
        [HttpPost]
        public void DeleteProduct(int productId)
        {
            dbProductService.DeleteProduct(productId);
        }
        [HttpGet]
        public IActionResult RedactProduct(int productId)
        {
            var userEmail = HttpContext.Session.GetString("Email");
            if (userEmail is null || !dbUserService.Get(userEmail).IsAdmin)
                return Redirect("../Home/Index");
            ViewBag.LoggedAccount = userEmail;
            ViewBag.IsAdmin = dbUserService.Get(userEmail).IsAdmin;
            ViewBag.User = dbUserService.Get(userEmail);
            ViewBag.UserBase = dbUserService.GetAll();
            ViewBag.Product = dbProductService.Get(productId);
            return View();
        }
        [HttpPost]
        public IActionResult RedactProduct(ProductViewModel newProduct, int productId)
        {
            dbProductService.RedactProduct(newProduct, productId);
            return RedirectToAction("AdminPanel");
        }
        public FileContentResult Download(string path)
        {
            var doc = new byte[0];
            doc = System.IO.File.ReadAllBytes(path);
            return File(doc, "application/pdf");
        }
        public void UpdateStatus(int applicationId, string value)
        {
            orderService.Update(applicationId, value);
        }
    }
}
