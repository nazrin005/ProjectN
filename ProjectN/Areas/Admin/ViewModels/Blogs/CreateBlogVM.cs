using System.ComponentModel.DataAnnotations;

namespace ProjectN.Areas.Admin.ViewModels.Blogs
{
    public class CreateBlogVM
    {
        [Required(ErrorMessage ="Title is required.")]
        [MaxLength(150)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [MaxLength(300)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; }
        public IFormFile ImageFile { get; set; }

        public bool IsFeatured { get; set; }
    }
}
