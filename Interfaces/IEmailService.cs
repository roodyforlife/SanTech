using SanTech.Models;

namespace SanTech.Interfaces
{
    public interface IEmailService
    {
        public void SendEmail(string email, string userName, string contentText, string filePath);
        public void SendCheckToEmail(Order application);
    }
}
