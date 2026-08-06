using System.ComponentModel.DataAnnotations;

namespace ProjectN.Areas.Admin.ViewModels.Blogs
{
    public class UpdateBlogVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        [StringLength(30, ErrorMessage = "Title can not exceed 30 chracters.")]
        [MinLength(3, ErrorMessage = "Title must be at least 3 symbols")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description can not exceed 500 chracters.")]
        [MinLength(10, ErrorMessage = "Description must be at least 10 symbols")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        [StringLength(50000, ErrorMessage = "Description can not exceed 50000 chracters.")]
        [MinLength(50, ErrorMessage = "Description must be at least 50 symbols")]
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile ImageFile { get; set; }

        public bool IsFeatured { get; set; }
    }
}
