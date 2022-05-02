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
        public EmailService(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.configuration = configuration;
        }
        //Email sender with file and html styles
        public void RegisterSend(string email, string userName)
        {
            try
            {
                string path = Path.Combine(this.hostingEnvironment.WebRootPath, "Files", "emailSendRegistration.html");
                string body = string.Empty;
                using (StreamReader reader = new StreamReader(path))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{userName}", userName);
                var senderEmail = new MailAddress(configuration["Smtp:FromAddress"], configuration["Smtp:Sender"]);
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
                mess.Subject = configuration["Smtp:Subject"];
                //mess.Body = message;
                //Attachment data = new Attachment(path, MediaTypeNames.Application.Octet);
                //mess.Attachments.Add(data);
                mess.IsBodyHtml = true;
                mess.Body = body;
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
