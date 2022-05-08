using SanTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanTech.Interfaces
{
    public interface IDbCommentService
    {
        public void Add(Comment comment, int productId);
        public void AddSub(SubComment subComment, int commentId);
        public List<Comment> Get(int productId);
        public Comment GetOne(int commentId);
    }
}
