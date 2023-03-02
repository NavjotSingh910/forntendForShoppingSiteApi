using System.ComponentModel.DataAnnotations;

namespace LoginPageTest.Models
{
    public class Items
    {
        [Key]
        public int Id { get; set; }
        public string? ItemName { get; set; }
        public decimal Rating { get; set; }
        public decimal Price { get; set; }
        public string? imageSource { get; set; }
    }
}
