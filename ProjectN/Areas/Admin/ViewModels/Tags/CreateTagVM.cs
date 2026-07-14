using System.ComponentModel.DataAnnotations;

namespace ProjectN.Areas.Admin.ViewModels.Tags
{
    public class CreateTagVM
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
    }
}
