using System.ComponentModel.DataAnnotations;

namespace PharmacyManagement.Models
{
    public class Prescription
    {
        public int PrescriptionId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(500)]
        public string FileUrl { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Notes { get; set; }

        [Required]
        [StringLength(30)]
        public string Status { get; set; } = "Pending";

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public Customer? Customer { get; set; }

        public ICollection<Purchase> Purchases { get; set; } =
            new List<Purchase>();
    }
}