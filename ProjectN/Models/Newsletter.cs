using ProjectN.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace ProjectN.Models
{
    public class Newsletter :BaseEntity
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        [MaxLength(100)]
        public string Email { get; set; }
    }
}
