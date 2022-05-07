using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanTech.Controllers
{
    public class CommentController : Controller
    {
        public IActionResult Comment()
        {
            return View();
        }
    }
}
