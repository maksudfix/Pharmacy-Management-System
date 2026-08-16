using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PharmacyManagement.Models.ViewModels.Prescription
{
    public class PrescriptionCreateEditViewModel
    {
        public int PrescriptionId { get; set; }

        [Required(ErrorMessage = "Customer is required.")]
        public int CustomerId { get; set; }

        [StringLength(500)]
        public string? FileUrl { get; set; }

        [Required(ErrorMessage = "Prescription file is required.")]
        public IFormFile? PrescriptionFile { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }

        [Required]
        [StringLength(30)]
        public string Status { get; set; } = "Pending";

        public IEnumerable<SelectListItem>? Customers { get; set; }
    }
}