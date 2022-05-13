using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SanTech.Interfaces;
using SanTech.Models;

namespace SanTech.Controllers
{
    public class CommentController : Controller
    {
        private readonly IDbUserService _dbUserService;
        private readonly IDbCommentService _dbCommentService;
        public CommentController(IDbUserService dbUserService, IDbCommentService dbCommentService)
        {
            _dbUserService = dbUserService;
            _dbCommentService = dbCommentService;
        }

        [HttpPost]
        public bool AddComment(string text, int evaluation, int productId)
        {
            if (text is not null && evaluation != 0)
            {
                var userEmail = HttpContext.Session.GetString("Email");
                Comment comment = new Comment()
                {
                    Text = text,
                    Evaluation = evaluation,
                    User = _dbUserService.Get(userEmail)
                };
                _dbCommentService.Add(comment, productId);
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
                    User = _dbUserService.Get(userEmail),
                    Comment = _dbCommentService.GetOne(commentId)
                };
                _dbCommentService.AddSub(subComment, commentId);
                return true;
            }

            return false;
        }

        public ViewResult LoadComments(int productId)
        {
            var userEmail = HttpContext.Session.GetString("Email");
            var model = _dbCommentService.Get(productId).OrderByDescending(x => x.Date);
            ViewBag.User = _dbUserService.Get(userEmail);
            return View(model);
        }

        public void DeleteComment(int commentId)
        {
            _dbCommentService.DeleteComment(commentId);
        }

        public void DeleteSubComment(int subCommentId)
        {
            _dbCommentService.DeleteSubComment(subCommentId);
        }
    }
}
