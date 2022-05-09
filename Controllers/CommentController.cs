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
        public bool AddComment(string text, int Evaluation, int productId)
        {
            if (text is not null && Evaluation != 0)
            {
                var userEmail = HttpContext.Session.GetString("Email");
                Comment comment = new Comment()
                {
                    Text = text,
                    Evaluation = Evaluation,
                    User = dbUserService.Get(userEmail)
                };
                dbCommentService.Add(comment, productId);
                return true;
            }
            return false;
        }
        public bool AddSubComment(string text, int commentId)
        {
            var userEmail = HttpContext.Session.GetString("Email");
            if (text is not null && userEmail is not null)
            {
                SubComment subComment = new SubComment()
                {
                    Text = text,
                    User = dbUserService.Get(userEmail),
                    Comment = dbCommentService.GetOne(commentId)
                };
                dbCommentService.AddSub(subComment, commentId);
                return true;
            }
            return false;
        }
        public ViewResult LoadComments(int productId)
        {
            var userEmail = HttpContext.Session.GetString("Email");
            var model = dbCommentService.Get(productId).OrderByDescending(x => x.Date);
            ViewBag.User = dbUserService.Get(userEmail);
            return View(model);
        }
        public void DeleteComment(int commentId)
        {
            dbCommentService.DeleteComment(commentId);
        }
        public void DeleteSubComment(int subCommentId)
        {
            dbCommentService.DeleteSubComment(subCommentId);
        }
    }
}
