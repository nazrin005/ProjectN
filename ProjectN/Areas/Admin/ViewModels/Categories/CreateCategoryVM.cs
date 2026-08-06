using System.ComponentModel.DataAnnotations;

namespace ProjectN.Areas.Admin.ViewModels.Categories
{
    public class CreateCategoryVM
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(30, ErrorMessage = "Name can not exceed 30 chracters.")]
        [MinLength(3, ErrorMessage = "Name must be at least 3 symbols")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description can not exceed 500 chracters.")]
        [MinLength(10, ErrorMessage = "Description must be at least 10 symbols")]
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        [Required(ErrorMessage = "Image is required")]
        public IFormFile ImageFile { get; set; }
    }
}
