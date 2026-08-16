using System.ComponentModel.DataAnnotations;

namespace PharmacyManagement.Models.ViewModels.Purchase
{
    public class PurchaseViewModel
    {
        public int PurchaseId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public int? PrescriptionId { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string DeliveryMethod { get; set; } = string.Empty;
        public string? DeliveryAddress { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime PurchaseDate { get; set; }
        public List<PurchaseItemViewModel> PurchaseItems { get; set; } = new();
    }
}