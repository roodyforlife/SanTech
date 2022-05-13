namespace SanTech.Models
{
    public class Favorite
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public User User { get; set; }
    }
}
