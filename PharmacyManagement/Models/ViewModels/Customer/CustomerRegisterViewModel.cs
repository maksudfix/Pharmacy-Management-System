using System.ComponentModel.DataAnnotations;

namespace PharmacyManagement.Models.ViewModels.Customer
{
    public class CustomerRegisterViewModel
    {
        [Required(ErrorMessage = "Customer name is required.")]
        [StringLength(150, ErrorMessage = "Name cannot exceed 150 characters.")]
        public string Name { get; set; } = string.Empty;

        [Range(1, 120, ErrorMessage = "Age must be between 1 and 120.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [StringLength(20)]
        public string Gender { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(20)]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [StringLength(300, ErrorMessage = "Address cannot exceed 300 characters.")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}