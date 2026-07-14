using ProjectN.Models.Base;

namespace ProjectN.Models
{
    public class Tag :BaseEntity
    {
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }
}
