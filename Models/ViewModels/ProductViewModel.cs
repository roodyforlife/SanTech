using Microsoft.AspNetCore.Http;

namespace SanTech.Models
{
    public class ProductViewModel
    {
        public string Title { get; set; }
        public string Desc { get; set; }
        public int SaleProcent { get; set; }
        public int BonusNumber { get; set; }
        public int Cost { get; set; }
        public IFormFile UploadedFile { get; set; }
        public bool IsNotAvailable { get; set; }
        public int CategoryId { get; set; }

    }
}
