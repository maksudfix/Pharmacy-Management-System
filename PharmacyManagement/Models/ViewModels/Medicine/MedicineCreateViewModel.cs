using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PharmacyManagement.Models.ViewModels.Medicine
{
    public class MedicineCreateViewModel
    {
        [Required(ErrorMessage = "Medicine name is required.")]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [StringLength(150)]
        public string? GenericName { get; set; }

        [StringLength(100)]
        public string? BrandName { get; set; }

        [StringLength(50)]
        public string? Strength { get; set; }

        [Required(ErrorMessage = "Dosage form is required.")]
        [StringLength(50)]
        public string DosageForm { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required.")]
        public int CategoryId { get; set; }

        [StringLength(200)]
        public string? Manufacturer { get; set; }

        public IFormFile? ImageFile { get; set; }

        public bool IsPrescriptionRequired { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public IEnumerable<SelectListItem>? Categories { get; set; }
    }
}