namespace PharmacyManagement.Models.ViewModels.Stock
{
    public class AdminSalesStockViewModel
    {
        public int MedicineId { get; set; }
        public string MedicineName { get; set; } = string.Empty;
        public int UnitsSold { get; set; }
        public int UnitsLeft { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal SellPrice { get; set; }
        public decimal TotalSalesAmount => UnitsSold * SellPrice;
        public decimal TotalBuyCost => UnitsSold * BuyPrice;
        public decimal ProfitAmount => TotalSalesAmount - TotalBuyCost;
    }
}