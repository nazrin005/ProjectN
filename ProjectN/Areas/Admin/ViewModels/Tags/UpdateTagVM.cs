using System.ComponentModel.DataAnnotations;

namespace ProjectN.Areas.Admin.ViewModels.Tags
{
    public class UpdateTagVM
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        public int Id { get; set; }
    }
}
