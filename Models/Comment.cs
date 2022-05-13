using System;
using System.Collections.Generic;

namespace SanTech.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public User User { get; set; }
        public int Evaluation { get; set; }
        public Product Product { get; set; }
        public List<SubComment> SubComments { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
