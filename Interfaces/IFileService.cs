using Microsoft.AspNetCore.Http;
using SanTech.Models;

namespace SanTech.Interfaces
{
    public interface IFileService
    {
        public byte[] FromImageToByte(IFormFile uploadedFile);
        public byte[] FromImageToByte(string imageFileName);
        public string GetHTMLBodyForCheck(Order application);
        public string GetCreatedPdfFile(Order application);
        public string HashData(string data);
        public string GenerateCode(User user);
    }
}
