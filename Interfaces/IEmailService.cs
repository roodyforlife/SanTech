using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanTech.Interfaces
{
    public interface IEmailService
    {
        public void RegisterSend(string email, string userName);
    }
}
