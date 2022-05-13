using SanTech.Models;
using System.Collections.Generic;

namespace SanTech.Interfaces
{
    public interface IDbCommentService
    {
        public void Add(Comment comment, int productId);
        public void AddSub(SubComment subComment, int commentId);
        public List<Comment> Get(int productId);
        public Comment GetOne(int commentId);
        public void DeleteComment(int commentId);
        public void DeleteSubComment(int subCommentId);
    }
}
