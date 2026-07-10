using ProjectN.Models.Base;

namespace ProjectN.Models
{
    public class Product :BaseEntity
    {
        public string Name { get; set; }  
        public decimal Price { get; set; }
        public Category Category { get; set; }
        public int? CategoryId { get; set; }
        public string? ImageUrl { get; set; }

    }
}
