using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment webHostEnvironment;
        public EmailService(IHostingEnvironment hostingEnvironment, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.configuration = configuration;
            this.webHostEnvironment = webHostEnvironment;
        }
        //Email sender with file and html styles
        public void RegisterSend(string email, string message, string filePath)
        {
            try
            {
                string body = string.Empty;
                using (StreamReader reader = new StreamReader("~/Files/test.docx"))
                {
                    body = reader.ReadToEnd();
                }
                var senderEmail = new MailAddress(configuration["Smtp:FromAddress"], "SanTech");
                var receiverEmail = new MailAddress(email, "Receiver");
                var smtp = new SmtpClient
                {
                    Host = configuration["Smtp:Server"],
                    Port = Convert.ToInt32(configuration["Smtp:Port"]),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail.Address, configuration["Smtp:Password"])
                };
                var mess = new MailMessage(senderEmail, receiverEmail);
                mess.Subject = "Administration";
                //mess.Body = message;
                string path = Path.Combine(this.hostingEnvironment.WebRootPath, "Files", $"{filePath}");
                Attachment data = new Attachment(path, MediaTypeNames.Application.Octet);
                mess.Attachments.Add(data);
                mess.IsBodyHtml = true;
                mess.Body = message;
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
