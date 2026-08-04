using System.ComponentModel.DataAnnotations;

namespace ProjectN.ViewModels.Review
{
    public record CreateReviewVM
    {
        [Required]
        [StringLength(500)]
        public string Comment { get; set; }

        [Range(1, 5)]
        public byte Rating { get; set; }

        public int ProductId { get; set; }
    }
}
