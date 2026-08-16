using System.ComponentModel.DataAnnotations;

namespace PharmacyManagement.Models.ViewModels.Category
{
    public class CategoryCreateEditViewModel
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }
    }
}