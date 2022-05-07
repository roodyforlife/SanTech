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
    }
}
