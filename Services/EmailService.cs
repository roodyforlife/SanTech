using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SanTech.Interfaces;
using SanTech.Models;
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
        private readonly IFileService fileService;
        public EmailService(IHostingEnvironment hostingEnvironment, IConfiguration configuration, IFileService fileService)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.configuration = configuration;
            this.fileService = fileService;
        }

        public void SendCheckToEmail(Order application)
        {
            try
            {
                var pdfPath = fileService.GetCreatedPdfFile(application);
                var senderEmail = new MailAddress(configuration["Smtp:FromAddress"], configuration["Smtp:Sender"]);
                var receiverEmail = new MailAddress(application.Email, "Receiver");
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
                Attachment data = new Attachment(pdfPath, MediaTypeNames.Application.Octet);
                mess.Attachments.Add(data);
                mess.IsBodyHtml = true;
                mess.Body = $"Спасибо за заказ №{application.OrderNumber}! Ниже прикреплен чек с вашими покупками. Удачных покупок!";
                {
                    smtp.Send(mess);
                }
            }
            catch
            {
            }
        }

        //Email sender with file and html styles
        public void SendEmail(string email, string userName, string contentText, string filePath)
        {
            try
            {
                string path = Path.Combine(this.hostingEnvironment.WebRootPath, "Files/email", filePath);
                string body = string.Empty;
                using (StreamReader reader = new StreamReader(path))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{userName}", userName).Replace("{contentText}", contentText);
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
