using System.ComponentModel.DataAnnotations;

namespace PharmacyManagement.Models.ViewModels.Stock
{
    public class StockViewModel
    {
        public int StockId { get; set; }
        public int MedicineId { get; set; }
        public string MedicineName { get; set; } = string.Empty;
        public string BatchNumber { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SellingPrice { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsExpired => ExpiryDate < DateTime.UtcNow;
    }
}