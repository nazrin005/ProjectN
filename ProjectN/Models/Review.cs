using ProjectN.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace ProjectN.Models
{
    public class Review : BaseEntity
    {
        [Required]
        [StringLength(500)]
        public string Comment { get; set; }
        [Range(1, 5)]
        public byte Rating { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
