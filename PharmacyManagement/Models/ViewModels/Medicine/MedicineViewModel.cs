using System.ComponentModel.DataAnnotations;

namespace PharmacyManagement.Models.ViewModels.Medicine
{
    public class MedicineViewModel
    {
        public int MedicineId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? GenericName { get; set; }
        public string? BrandName { get; set; }
        public string? Strength { get; set; }
        public string DosageForm { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string? Manufacturer { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsPrescriptionRequired { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public int TotalStockQuantity { get; set; }
        public decimal LatestSellingPrice { get; set; }
    }
}