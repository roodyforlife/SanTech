using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SanTech.Interfaces;
using SanTech.Models;

namespace SanTech.Controllers
{
    public class AdminPanelController : Controller
    {
        private readonly IDbProductService _dbProductService;
        private readonly IFileService _fileService;
        private readonly IDbUserService _dbUserService;
        private readonly IOrderService _orderService;
        public AdminPanelController(IDbProductService dbProductService, IFileService fileService, IDbUserService dbUserService, IOrderService orderService)
        {
            _dbProductService = dbProductService;
            _fileService = fileService;
            _dbUserService = dbUserService;
            _orderService = orderService;
        }

        public IActionResult AdminPanel()
            {
            var userEmail = HttpContext.Session.GetString("Email");
            if (userEmail is null || !_dbUserService.Get(userEmail).IsAdmin)
            {
                return Redirect("../Home/Index");
            }

            ViewBag.User = _dbUserService.Get(userEmail);
            ViewBag.UserBase = _dbUserService.GetAll();
            ViewBag.ApplicationsBase = _orderService.Get().OrderByDescending(x => x.Id);
            var allProducts = _dbProductService.GetAll();
            return View(_dbProductService.GetProductsInRange(0, 20, allProducts).ToList());
        }

        public bool AddNewProduct(ProductViewModel product)
        {
            if (product.Title is null || product.Desc is null || product.Cost == 0 ||
                product.UploadedFile is null || product.SaleProcent < 0 ||
                product.SaleProcent > 100 || product.CategoryId == 0)
            {
                return true;
            }

            _dbProductService.Add(product);
            return false;
        }

        [HttpPost]
        public ViewResult GetAdditionalProducts(int from, int count)
        {
            var allProducts = _dbProductService.GetAll();
            var products = _dbProductService.GetProductsInRange(from, count, allProducts).ToList();
            return View(products);
        }

        [HttpPost]
        public void DeleteProduct(int productId)
        {
            _dbProductService.DeleteProduct(productId);
        }

        [HttpGet]
        public IActionResult RedactProduct(int productId)
        {
            var userEmail = HttpContext.Session.GetString("Email");
            if (userEmail is null || !_dbUserService.Get(userEmail).IsAdmin)
            {
                return Redirect("../Home/Index");
            }

            ViewBag.LoggedAccount = userEmail;
            ViewBag.IsAdmin = _dbUserService.Get(userEmail).IsAdmin;
            ViewBag.User = _dbUserService.Get(userEmail);
            ViewBag.UserBase = _dbUserService.GetAll();
            ViewBag.Product = _dbProductService.Get(productId);
            return View();
        }

        [HttpPost]
        public IActionResult RedactProduct(ProductViewModel newProduct, int productId)
        {
            _dbProductService.RedactProduct(newProduct, productId);
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
            _orderService.Update(applicationId, value);
        }
    }
}
