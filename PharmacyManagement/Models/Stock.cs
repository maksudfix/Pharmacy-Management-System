using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyManagement.Models
{
    public class Stock
    {
        public int StockId { get; set; }

        [Required]
        public int MedicineId { get; set; }

        [Required]
        [StringLength(50)]
        public string BatchNumber { get; set; } = string.Empty;

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue)]
        public decimal PurchasePrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue)]
        public decimal SellingPrice { get; set; }

        public DateTime ManufacturingDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Medicine? Medicine { get; set; }
    }
}