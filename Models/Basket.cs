namespace SanTech.Models
{
    public class Basket
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public User User { get; set; }
        public int NumberOfProduct { get; set; }
    }
}
