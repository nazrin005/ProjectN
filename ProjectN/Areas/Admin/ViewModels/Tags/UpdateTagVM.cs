using System.ComponentModel.DataAnnotations;

namespace ProjectN.Areas.Admin.ViewModels.Tags
{
    public class UpdateTagVM
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(30, ErrorMessage = "Name can not exceed 30 chracters.")]
        [MinLength(3, ErrorMessage = "Name must be at least 3 symbols")]
        public string Name { get; set; }
        public int Id { get; set; }
    }
}
