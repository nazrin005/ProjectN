using System.ComponentModel.DataAnnotations;

namespace ProjectN.ViewModels.Contact
{
    public class CreateContactMessageVM
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        [MaxLength(100)]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [MaxLength(1000)]
        public string Message { get; set; }
    }
}
