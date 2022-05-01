using Microsoft.AspNetCore.Hosting;
using MimeKit;
using SanTech.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace SanTech.Services
{
    public class EmailService : IEmailService
    {
        private readonly IHostingEnvironment hostingEnvironment;
        public EmailService(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }
        //Email sender with file and html styles
        public void RegisterSend(string email, string message)
        {
            try
            {
                var senderEmail = new MailAddress("roodyhacker123@gmail.com", "SanTech");
                var receiverEmail = new MailAddress(email, "Receiver");
                var password = "m@iTT7Rd1UBILsBueWX7";
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail.Address, password)
                };
                var mess = new MailMessage(senderEmail, receiverEmail);
                mess.Subject = "Administration";
                //mess.Body = message;
                string path = Path.Combine(this.hostingEnvironment.WebRootPath, "Files", "test.docx");
                Attachment data = new Attachment(path, MediaTypeNames.Application.Octet);
                mess.Attachments.Add(data);
                mess.IsBodyHtml = true;
                mess.Body = $"<div style='color: red; font-size: 40px;'>sdfsdf</div>";
                {
                    smtp.Send(mess);
                }
            }
            catch
            {
            }
        }
    }
}
