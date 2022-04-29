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
        public AdminPanelController(IDbProductService dbProductService, IFileService fileService)
        {
            this.dbProductService = dbProductService;
            this.fileService = fileService;
        }
        public IActionResult AdminPanel()
        {
            return View();
        }
        [HttpPost]
        public void AdminPanel12(string title, string desc, int saleProcent, int bonusNumber, int cost, IFormFile uploadedFile)
        {
            Product product = new Product { Title = title, Desc = desc, SaleProcent = saleProcent, BonusNumber = bonusNumber, Cost = cost};
            product.Image = fileService.FromImageToByte(uploadedFile);
            dbProductService.Add(product);
        }
    }
}
