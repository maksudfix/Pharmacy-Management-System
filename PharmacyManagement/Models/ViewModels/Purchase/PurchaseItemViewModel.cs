using System.ComponentModel.DataAnnotations;

namespace PharmacyManagement.Models.ViewModels.Purchase
{
    public class PurchaseItemViewModel
    {
        public int PurchaseItemId { get; set; }
        public int MedicineId { get; set; }
        public string MedicineName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}