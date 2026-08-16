using System.ComponentModel.DataAnnotations;

namespace PharmacyManagement.Models.ViewModels.Category
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int MedicineCount { get; set; }
    }
}