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
    public class CommentController : Controller
    {
        private readonly IDbUserService dbUserService;
        private readonly IDbCommentService dbCommentService;
        public CommentController(IDbUserService dbUserService, IDbCommentService dbCommentService)
        {
            this.dbUserService = dbUserService;
            this.dbCommentService = dbCommentService;
        }
        [HttpPost]
        public void AddComment(string text, int Evaluation, int productId)
        {

            var userEmail = HttpContext.Session.GetString("Email");
            Comment comment = new Comment() {
                Text = text,
                Evaluation = Evaluation,
                User = dbUserService.Get(userEmail)
            };
            dbCommentService.Add(comment, productId);
        }
    }
}
