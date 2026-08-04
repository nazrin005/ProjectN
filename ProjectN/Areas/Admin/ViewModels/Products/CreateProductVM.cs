using ProjectN.Models;
using System.ComponentModel.DataAnnotations;

namespace ProjectN.Areas.Admin.ViewModels.Products
{
    public class CreateProductVM
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(30, ErrorMessage = "Name can not exceed 30 chracters.")]
        [MinLength(3, ErrorMessage = "Name must be at least 3 symbols")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description can not exceed 500 chracters.")]
        [MinLength(3, ErrorMessage = "Description must be at least 10 symbols")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Tag is required")]
        public List<int> TagIds { get; set; }
        [Required(ErrorMessage = "Category is required")]
        public int? CategoryId { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile ImageFile { get; set; }
        public List<IFormFile>? ImageFiles { get; set; }
    }
}
