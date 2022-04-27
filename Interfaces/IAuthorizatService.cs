using SanTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanTech.Interfaces
{
    public interface IAuthorizatService
    {
        public bool IsRegistered(string login);
        public bool PasswordIsCorrect(string login, string password);
    }
}
