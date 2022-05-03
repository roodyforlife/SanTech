using IronPdf;
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
        public EmailService(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.configuration = configuration;
        }

        public void SendCheckToEmail(Application application)
        {
            try
            {
                string path = Path.Combine(this.hostingEnvironment.WebRootPath, "Files/email", "Check.html");
                string body = string.Empty;
                string basket = String.Empty;
                //Get Structure of body
                using (StreamReader reader = new StreamReader(path))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{OrderNumber}", application.OrderNumber).Replace("{UserName}", application.Name).Replace("{UserSecondName}", application.SecondName).
                    Replace("{City}", application.City).Replace("{Address}", application.Address).Replace("{Post}", application.Delivery).
                    Replace("{TotalCost}", Convert.ToString(application.TotalCost));
                foreach(var item in application.User.Basket)
                {
                    basket += $"<div class='content__basket__item'><div class='text1'>{item.Product.Title}</div><div class='text1 content__basket__item__code'>Код товара: {item.Product.Id}</div><div class='text1'>Количество: {item.NumberOfProduct}</div><div class='text1 content__basket__item__cost'>{(item.Product.Cost * item.NumberOfProduct * (100 - item.Product.SaleProcent) / 100).ToString("N0")} грн. </div></div>";
                    basket += $" <img src='data: image / jpeg; base64,{(Convert.ToBase64String(item.Product.Image))}'>";
                }
                body = body.Replace("{BasketContent}", basket);
                //End
                //PDF save
                var pdfDocument = new HtmlToPdf().RenderHtmlAsPdf(body);
                string pdfPath = Path.Combine(this.hostingEnvironment.WebRootPath, $"Files/Orders/{application.OrderNumber}.pdf");
                pdfDocument.SaveAs(pdfPath);
                //End
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
