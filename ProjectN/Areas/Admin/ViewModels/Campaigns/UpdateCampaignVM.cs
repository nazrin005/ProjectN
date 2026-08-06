using System.ComponentModel.DataAnnotations;

namespace ProjectN.Areas.Admin.ViewModels.Campaigns
{
    public class UpdateCampaignVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(30, ErrorMessage = "Title can not exceed 30 chracters.")]
        [MinLength(3, ErrorMessage = "Title must be at least 3 symbols")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Subtitle is required")]
        [StringLength(60, ErrorMessage = "Subtitle can not exceed 60 chracters.")]
        [MinLength(3, ErrorMessage = "Subtitle must be at least 3 symbols")]
        public string Subtitle { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description can not exceed 500 chracters.")]
        [MinLength(10, ErrorMessage = "Description must be at least 10 symbols")]
        public string Description { get; set; }

        [Required(ErrorMessage = "ButtonText is required")]
        [StringLength(20, ErrorMessage = "text can not exceed 20 chracters.")]
        [MinLength(2, ErrorMessage = "text must be at least 2 symbols")]
        public string ButtonText { get; set; }

        [Required(ErrorMessage = "ButtonUrl is required")]
        public string ButtonUrl { get; set; }

        public string? ImageUrl { get; set; }

        public IFormFile? ImageFile { get; set; }

        public bool IsActive { get; set; }
    }
}
