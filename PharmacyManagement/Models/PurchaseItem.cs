using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyManagement.Models
{
    public class PurchaseItem
    {
        public int PurchaseItemId { get; set; }

        [Required]
        public int PurchaseId { get; set; }

        [Required]
        public int MedicineId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        public Purchase? Purchase { get; set; }

        public Medicine? Medicine { get; set; }
    }
}