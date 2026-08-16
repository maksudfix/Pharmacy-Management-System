using PharmacyManagement.Models.ViewModels.Purchase;

namespace PharmacyManagement.Models.ViewModels.Admin
{
    public class CustomerPurchaseHistoryAdminViewModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public List<PurchaseOrderViewModel> Purchases { get; set; } = new List<PurchaseOrderViewModel>();
    }

    public class PurchaseOrderViewModel
    {
        public int PurchaseId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<PurchaseItemViewModel> PurchaseItems { get; set; } = new List<PurchaseItemViewModel>();
    }
}