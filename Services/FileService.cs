using Microsoft.AspNetCore.Http;
using SanTech.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SanTech.Services
{
    public class FileService : IFileService
    {
        public byte[] FromImageToByte(IFormFile uploadedFile)
        {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(uploadedFile.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)uploadedFile.Length);
                }
            return imageData;
        }
    }
}
