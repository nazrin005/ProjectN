using ProjectN.Models.Base;

namespace ProjectN.Models
{
    public class Category : BaseEntity
    {
        public List<Product> Products { get; set; }
    }
}
