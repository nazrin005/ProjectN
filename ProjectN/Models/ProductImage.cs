using ProjectN.Models.Base;

namespace ProjectN.Models
{
    public class ProductImage : BaseEntity
    {
        public string ImageUrl { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
