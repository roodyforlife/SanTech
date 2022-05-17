using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using IronPdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SanTech.Interfaces;
using SanTech.Models;

namespace SanTech.Services
{
    public class FileService : IFileService
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public FileService(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public byte[] FromImageToByte(IFormFile uploadedFile)
        {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(uploadedFile.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)uploadedFile.Length);
                }

                return imageData;
        }

        public byte[] FromImageToByte(string imageFileName)
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "img", imageFileName);
            FileStream fileStream = File.OpenRead(path);
            BinaryReader reader = new BinaryReader(fileStream);
            return reader.ReadBytes((int)fileStream.Length);
        }

        // Create Pdf file for created order
        public string GetCreatedPdfFile(Order application)
        {
            var pdfDocument = new HtmlToPdf().RenderHtmlAsPdf(GetHTMLBodyForCheck(application));
            string pdfPath = Path.Combine(_hostingEnvironment.WebRootPath, $"Files/Orders/{application.OrderNumber}.pdf");
            pdfDocument.SaveAs(pdfPath);
            return pdfPath;
        }

        // Make a check for Pdf file
        public string GetHTMLBodyForCheck(Order application)
        {
            string body = string.Empty;
            StringBuilder basket = new StringBuilder();
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "Files/email", "Check.html");
            using (StreamReader reader = new StreamReader(path))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{OrderNumber}", application.OrderNumber).Replace("{UserName}", application.Name).Replace("{UserSecondName}", application.SecondName).
                Replace("{City}", application.City).Replace("{Address}", application.Address).Replace("{Post}", application.Delivery).
                Replace("{TotalCost}", application.TotalCost.ToString("N0"));
            foreach (var item in application.User.Basket)
            {
                basket.Append($"<div class='content__basket__item'><div class='text1'>{item.Product.Title}</div>" +
                    $"<div class='text1 content__basket__item__code'>Код товара: {item.Product.Id}</div>" +
                    $"<div class='text1'>Количество: {item.NumberOfProduct}</div>" +
                    $"<div class='text1 content__basket__item__cost'>{(item.NumberOfProduct * (item.Product.Cost * (100 - item.Product.SaleProcent) / 100)).ToString("N0")} грн.</div></div>");
            }

            body = body.Replace("{BasketContent}", basket.ToString());
            return body;
        }

        // Data hashing
        public string HashData(string data)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hashedBytes).Replace("-", string.Empty).ToLower();
            }
        }

        public string GenerateCode(User user)
        {
            if (user is null)
                return null;
            string data = user.Email + user.Phone + user.Password;
            return HashData(data);
        }
    }
}
