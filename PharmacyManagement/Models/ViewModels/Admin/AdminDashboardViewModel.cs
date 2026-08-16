using PharmacyManagement.Models.ViewModels.Stock;

namespace PharmacyManagement.Models.ViewModels.Admin
{
    public class AdminDashboardViewModel
    {
        public int TotalCustomers { get; set; }
        public int TotalMedicinesStock { get; set; }
        public List<CustomerListViewModel> Customers { get; set; } = new();
        public List<AdminSalesStockViewModel> SalesStocks { get; set; } = new();
    }

    public class CustomerListViewModel
    {
        public int CustomerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int TotalMedicinesBought { get; set; }
    }

    public class CustomerPurchaseHistoryViewModel
    {
        public int PurchaseId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<PurchaseItemDetailViewModel> PurchaseItems { get; set; } = new();
    }

    public class PurchaseItemDetailViewModel
    {
        public string MedicineName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
    }
}