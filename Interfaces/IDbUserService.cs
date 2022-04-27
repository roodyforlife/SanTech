using SanTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanTech.Interfaces
{
    public interface IDbUserService
    {
        public void Add(User user);
        public User Get(string user);
    }
}
