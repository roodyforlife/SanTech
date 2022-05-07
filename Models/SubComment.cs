﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanTech.Models
{
    public class SubComment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public User User { get; set; }
        public int Evaluation { get; set; }
        public Product Product { get; set; }
        public DateTime Date = DateTime.Now;
    }
}
