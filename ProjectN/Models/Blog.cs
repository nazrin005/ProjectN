using ProjectN.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace ProjectN.Models
{
    public class Blog : BaseEntity
    {
        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        [Required]
        [MaxLength(300)]
        public string Description { get; set; }

        [Required]
        public string Content { get; set; }

        public string? ImageUrl { get; set; }

        [MaxLength(50)]
        public string Author { get; set; } = "Admin";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsFeatured { get; set; }
    }
}
