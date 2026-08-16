using System.ComponentModel.DataAnnotations;

namespace PharmacyManagement.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [Range(1, 120)]
        public int Age { get; set; }

        [Required]
        [StringLength(20)]
        public string Gender { get; set; } = string.Empty;

        [Phone]
        [StringLength(20)]
        public string? Phone { get; set; }

        [EmailAddress]
        [StringLength(150)]
        public string? Email { get; set; }

        [StringLength(300)]
        public string? Address { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Purchase> Purchases { get; set; } =
            new List<Purchase>();

        public ICollection<Prescription> Prescriptions { get; set; } =
            new List<Prescription>();
    }
}