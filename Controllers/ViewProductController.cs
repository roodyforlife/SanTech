using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SanTech.Interfaces;

namespace SanTech.Controllers
{
    public class ViewProductController : Controller
    {
        private readonly IDbUserService _dbUserService;
        private readonly IDbProductService _dbProductService;
        public ViewProductController(IDbUserService dbUserService, IDbProductService dbProductService)
        {
            _dbUserService = dbUserService;
            _dbProductService = dbProductService;
        }

        public IActionResult ViewProduct(int productId)
        {
            try
            {
                var userEmail = HttpContext.Session.GetString("Email");
                if (userEmail is not null)
                {
                    ViewBag.User = _dbUserService.Get(userEmail);
                }

                var model = _dbProductService.Get(productId);
                model.Comments = model.Comments.OrderByDescending(x => x.Id).ToList();
                return View(model);
            }
            catch
            {
                return Redirect("../Home/Index");
            }
        }
    }
}
