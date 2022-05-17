using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SanTech.Interfaces;
using SanTech.Models;

namespace SanTech.Services
{
    public class DbCommentService : IDbCommentService
    {
        private readonly IDbProductService _dbProductService;
        private readonly DataBaseContext _db;
        public DbCommentService(IDbProductService dbProductService, DataBaseContext db)
        {
            _dbProductService = dbProductService;
            _db = db;
        }

        public void Add(Comment comment, int productId)
        {
            comment.Product = _dbProductService.Get(productId);
            _db.Comments.Add(comment);
            _db.SaveChanges();
        }

        public void AddSub(SubComment subComment, int commentId)
        {
            _db.SubComments.Add(subComment);
            _db.SaveChanges();
        }

        public void DeleteComment(int commentId)
        {
            var comment = GetOne(commentId);
            if (comment.SubComments.Count() == 0)
            {
                _db.Comments.Remove(comment);
            }
            else
            {
                comment.Text = "(Удалено)";
            }

            _db.SaveChanges();
        }

        public void DeleteSubComment(int subCommentId)
        {
            var subComment = _db.SubComments.ToList().FirstOrDefault(x => x.Id == subCommentId);
            _db.SubComments.Remove(subComment);
            _db.SaveChanges();
        }

        public List<Comment> Get(int productId)
        {
            return _db.Comments.Include(x => x.Product).Include(x => x.User).
                Include(x => x.SubComments).Where(x => x.Product.Id == productId).ToList();
        }

        public Comment GetOne(int commentId)
        {
            return _db.Comments.Include(x => x.SubComments).ToList().FirstOrDefault(x => x.Id == commentId);
        }
    }
}
