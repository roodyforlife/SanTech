using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using SanTech.Interfaces;
using SanTech.Models;

namespace SanTech.Services
{
    public class EmailService : IEmailService
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IFileService _fileService;
        public EmailService(IHostingEnvironment hostingEnvironment, IConfiguration configuration, IFileService fileService)
        {
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _fileService = fileService;
        }

        public void SendCheckToEmail(OrderViewModel application)
        {
            try
            {
                var pdfPath = _fileService.GetCreatedPdfFile(application);
                var senderEmail = new MailAddress(_configuration["Smtp:FromAddress"], _configuration["Smtp:Sender"]);
                var receiverEmail = new MailAddress(application.Email, "Receiver");
                var smtp = new SmtpClient
                {
                    Host = _configuration["Smtp:Server"],
                    Port = Convert.ToInt32(_configuration["Smtp:Port"]),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail.Address, _configuration["Smtp:Password"])
                };
                var mess = new MailMessage(senderEmail, receiverEmail);
                mess.Subject = _configuration["Smtp:Subject"];
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

        // Email sender with file and html styles
        public void SendEmail(string email, string userName, string contentText, string filePath)
        {
            try
            {
                string path = Path.Combine(_hostingEnvironment.WebRootPath, "Files/email", filePath);
                string body = string.Empty;
                using (StreamReader reader = new StreamReader(path))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{userName}", userName).Replace("{contentText}", contentText);
                var senderEmail = new MailAddress(_configuration["Smtp:FromAddress"], _configuration["Smtp:Sender"]);
                var receiverEmail = new MailAddress(email, "Receiver");
                var smtp = new SmtpClient
                {
                    Host = _configuration["Smtp:Server"],
                    Port = Convert.ToInt32(_configuration["Smtp:Port"]),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail.Address, _configuration["Smtp:Password"])
                };
                var mess = new MailMessage(senderEmail, receiverEmail);
                mess.Subject = _configuration["Smtp:Subject"];
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
