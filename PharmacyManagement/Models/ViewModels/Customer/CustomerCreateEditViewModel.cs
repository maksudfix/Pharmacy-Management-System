using System.ComponentModel.DataAnnotations;

namespace PharmacyManagement.Models.ViewModels.Customer
{
    public class CustomerCreateEditViewModel
    {
        public int CustomerId { get; set; }

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

        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [StringLength(150)]
        public string? Email { get; set; }

        [StringLength(300, ErrorMessage = "Address cannot exceed 300 characters.")]
        public string? Address { get; set; }
    }
}