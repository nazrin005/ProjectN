using Microsoft.AspNetCore.Identity;

namespace ProjectN.Models
{
    public class AppUser :IdentityUser
    {
        public string Name { get; set; }
        public  string Surname { get; set; }
        public  bool IsAdmin { get; set; }
        public bool IsDeleted { get; set; }
        public List<Review> Reviews { get; set; }
        public List<Wishlist> Wishlists { get; set; }
        public List<Cart> Carts { get; set; }
        public List<Order>? Orders { get; set; }
    }
}
