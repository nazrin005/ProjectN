using ProjectN.Models.Base;

namespace ProjectN.Models
{
    public class Cart :BaseEntity
    {
         public int Count { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
