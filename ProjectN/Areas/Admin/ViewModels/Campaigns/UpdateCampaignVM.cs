using System.ComponentModel.DataAnnotations;

namespace ProjectN.Areas.Admin.ViewModels.Campaigns
{
    public class UpdateCampaignVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Subtitle is required")]
        public string Subtitle { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "ButtonText is required")]
        public string ButtonText { get; set; }

        [Required(ErrorMessage = "ButtonUrl is required")]
        public string ButtonUrl { get; set; }

        public string? ImageUrl { get; set; }

        public IFormFile? ImageFile { get; set; }

        public bool IsActive { get; set; }
    }
}
