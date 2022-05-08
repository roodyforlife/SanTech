using Microsoft.EntityFrameworkCore;
using SanTech.Interfaces;
using SanTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        /*public void DeleteAllComments(int productId)
        {
            var subComments = db.SubComments.ToList().Where(x => x.Comment.Product.Id == productId);
            var comments = db.Comments.ToList().Where(x => x.Product.Id == productId);
            db.SubComments.RemoveRange(subComments);
            db.Comments.RemoveRange(comments);
        }*/

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
