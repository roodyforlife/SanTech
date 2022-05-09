﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanTech.Models.ViewModels
{
    public class SearchViewModel
    {
        public int Category { get; set; }
        public string Sort { get; set; }
        public string SearchInput { get; set; }
        public int CostFrom { get; set; }
        public int CostTo { get; set; }
    }
}
