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
            var isAdmin = dbUserService.Get(user).IsAdmin;
            if(!isAdmin)
                return Redirect("../Home/Index");
            ViewBag.LoggedAccount = user;
            if (user is not null)
            {
                ViewBag.IsAdmin = isAdmin;
                ViewBag.User = dbUserService.Get(user);
            }
            return View();
        }
        [HttpPost]
        public void AdminPanel12(string product)
        {
            /*if(Title is null || Desc is null || Cost == 0 || UploadedFile is null)
                return true;
            Product product = new Product { Title = Title, Desc = Desc, SaleProcent = SaleProcent, BonusNumber =BonusNumber, Cost = Cost};
            product.Image = fileService.FromImageToByte(UploadedFile);
            dbProductService.Add(product);
            return false;*/
        }
    }
}
