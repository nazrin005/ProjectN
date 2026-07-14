using System.ComponentModel.DataAnnotations;

namespace ProjectN.Areas.Admin.ViewModels.Categories
{
    public class CreateCategoryVM
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
    }
}
