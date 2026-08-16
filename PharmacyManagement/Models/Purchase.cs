using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyManagement.Models
{
    public class Purchase
    {
        public int PurchaseId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public int? PrescriptionId { get; set; }

        [Required]
        [StringLength(30)]
        public string PaymentMethod { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string DeliveryMethod { get; set; } = string.Empty;

        [StringLength(300)]
        public string? DeliveryAddress { get; set; }

        [Required]
        [StringLength(30)]
        public string Status { get; set; } = "Pending";

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
        public Customer? Customer { get; set; }

        public Prescription? Prescription { get; set; }

        public ICollection<PurchaseItem> PurchaseItems { get; set; } =
            new List<PurchaseItem>();
    }
}