using Microsoft.EntityFrameworkCore;
using SanTech.Interfaces;
using SanTech.Models;
using System.Collections.Generic;
using System.Linq;

namespace SanTech.Services
{
    public class DbCommentService : IDbCommentService
    {
        private readonly IDbProductService dbProductService;
        private readonly ApplicationContext db;
        public DbCommentService(IDbProductService dbProductService, ApplicationContext db)
        {
            this.dbProductService = dbProductService;
            this.db = db;
        }

        public void Add(Comment comment, int productId)
        {
            comment.Product = dbProductService.Get(productId);
            db.Comments.Add(comment);
            db.SaveChanges();
        }

        public void AddSub(SubComment subComment, int commentId)
        {
            db.SubComments.Add(subComment);
            db.SaveChanges();
        }

        public void DeleteComment(int commentId)
        {
            var comment = GetOne(commentId);
            if (comment.SubComments.Count() == 0)
                db.Comments.Remove(comment);
            else
                comment.Text = "(Удалено)";
            db.SaveChanges();
            
        }

        public void DeleteSubComment(int subCommentId)
        {
            var subComment = db.SubComments.ToList().FirstOrDefault(x => x.Id == subCommentId);
            db.SubComments.Remove(subComment);
            db.SaveChanges();
        }

        public List<Comment> Get(int productId)
        {
            return db.Comments.Include(x => x.Product).Include(x => x.User).Include(x => x.SubComments).Where(x => x.Product.Id == productId).ToList();
        }

        public Comment GetOne(int commentId)
        {
            return db.Comments.Include(x => x.SubComments).ToList().FirstOrDefault(x => x.Id == commentId);
        }
    }
}
