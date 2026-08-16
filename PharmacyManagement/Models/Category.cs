using System.ComponentModel.DataAnnotations;

namespace PharmacyManagement.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }
        public ICollection<Medicine> Medicines { get; set; } = new List<Medicine>();
    }
}