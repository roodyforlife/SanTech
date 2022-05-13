using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SanTech.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public int SaleProcent { get; set; }
        public int BonusNumber { get; set; }
        public int Cost { get; set; }
        public byte[] Image { get; set; }
        public bool IsNotAvailable { get; set; }
        public List<Comment> Comments { get; set; }
        public int CategoryId { get; set; }
        public Product(string Title, string Desc, int SaleProcent, int BonusNumber, int Cost, byte[] Image, int CategoryId)
        {
            this.Title = Title;
            this.Desc = Desc;
            this.SaleProcent = SaleProcent;
            this.BonusNumber = BonusNumber;
            this.Cost = Cost;
            this.Image = Image;
            this.CategoryId = CategoryId;
        }
    }
}
