using ProjectN.Models.Base;

namespace ProjectN.Models
{
    public class Order :BaseEntity
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public decimal TotalPrice { get; set; }
        public List<OrderItem>? OrderItems { get; set; }
    }
}
