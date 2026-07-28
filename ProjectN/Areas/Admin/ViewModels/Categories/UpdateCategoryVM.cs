using System.ComponentModel.DataAnnotations;

namespace ProjectN.Areas.Admin.ViewModels.Categories
{
    public class UpdateCategoryVM
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        [Required(ErrorMessage = "Image is required")]
        public IFormFile ImageFile { get; set; }
        public int Id { get; set; }
    }
}
