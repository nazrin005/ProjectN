using System.ComponentModel.DataAnnotations;

namespace ProjectN.ViewModels.Account
{
    public record RegisterVM
    {

        [Required(ErrorMessage = "Username is required")]
        [StringLength(20, ErrorMessage = "Username can not exceed 20 characters")]
        [MinLength(3, ErrorMessage = "Username can not exceed 3 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(20, ErrorMessage = "Name can not exceed 20 characters")]
        [MinLength(3, ErrorMessage = "Name can not exceed 3 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        [StringLength(20, ErrorMessage = "Surname can not exceed 20 characters")]
        [MinLength(3, ErrorMessage = "Surname can not exceed 3 characters")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(20, ErrorMessage = "Email can not exceed 20 characters")]
        [MinLength(3, ErrorMessage = "Email can not exceed 3 characters")]
        [EmailAddress(ErrorMessage = "Email is not valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Password do not match")]
        public string ConfirmPassword { get; set; }

    }
}
