using ProjectN.Models.Base;

namespace ProjectN.Models
{
    public class Product :BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public List<Tag> Tags { get; set; }
        public Category Category { get; set; }
        public int? CategoryId { get; set; }
        public string? ImageUrl { get; set; }
        public List<Review> Reviews { get; set; }
        public List<Wishlist> Wishlists { get; set; }
        public List<Cart> Carts { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new();
        public List<ProductImage> ProductImages { get; set; } = new();
    }
}
