using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanTech.Models
{
    public class Application
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SecondName { get; set; }
        public int Phone { get; set; }
        public string Email { get; set; }
        public string FilePath { get; set; }
        public User User { get; set; }
        public int BonusCredit { get; set; }
        public string Status { get; set; }
    }
}
