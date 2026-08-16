using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace PharmacyManagement.Models
{
    public class Medicine
    {
        public int MedicineId { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [StringLength(150)]
        public string? GenericName { get; set; }

        [StringLength(100)]
        public string? BrandName { get; set; }

        [StringLength(50)]
        public string? Strength { get; set; }

        [Required]
        [StringLength(50)]
        public string DosageForm { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }

        [StringLength(200)]
        public string? Manufacturer { get; set; }

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        public bool IsPrescriptionRequired { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
        public Category? Category { get; set; }

        public ICollection<Stock> Stocks { get; set; } = new List<Stock>();

        public ICollection<PurchaseItem> PurchaseItems { get; set; } =
            new List<PurchaseItem>();
    }
}