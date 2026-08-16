using System.ComponentModel.DataAnnotations;

namespace PharmacyManagement.Models.ViewModels.Cart
{
    public class CartItemViewModel
    {
        [Required]
        public int MedicineId { get; set; }

        public string MedicineName { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        public int StockLimit { get; set; }
        public decimal TotalPrice
        {
            get { return Quantity * UnitPrice; }
        }
    }
}