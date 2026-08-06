using System.ComponentModel.DataAnnotations;

namespace ProjectN.ViewModels.Review
{
    public record CreateReviewVM
    {
        [Required]
        [StringLength(500, ErrorMessage = "Comment can not exceed 500 chracters.")]
        [MinLength(3, ErrorMessage = "Comment must be at least 10 symbols")]
        public string Comment { get; set; }

        [Range(1, 5)]
        public byte Rating { get; set; }

        public int ProductId { get; set; }
    }
}
