using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PharmacyManagement.Models.ViewModels.Stock
{
    public class StockCreateEditViewModel
    {
        public int StockId { get; set; }

        [Required(ErrorMessage = "Medicine is required.")]
        public int MedicineId { get; set; }

        [Required(ErrorMessage = "Batch number is required.")]
        [StringLength(50)]
        public string BatchNumber { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be 0 or greater.")]
        public int Quantity { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Purchase price must be greater than zero.")]
        public decimal PurchasePrice { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Selling price must be greater than zero.")]
        public decimal SellingPrice { get; set; }

        [Required(ErrorMessage = "Manufacturing date is required.")]
        public DateTime ManufacturingDate { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "Expiry date is required.")]
        public DateTime ExpiryDate { get; set; } = DateTime.UtcNow.AddMonths(12);

        public IEnumerable<SelectListItem>? Medicines { get; set; }
    }
}