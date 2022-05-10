using SanTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanTech.Interfaces
{
    public interface IEmailService
    {
        public void SendEmail(string email, string userName, string contentText, string filePath);
        public void SendCheckToEmail(Order application);
    }
}
