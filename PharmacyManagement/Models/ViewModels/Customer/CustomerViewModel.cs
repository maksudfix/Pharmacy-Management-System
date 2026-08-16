using System.ComponentModel.DataAnnotations;

namespace PharmacyManagement.Models.ViewModels.Customer
{
    public class CustomerViewModel
    {
        public int CustomerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int TotalPurchases { get; set; }
        public int TotalMedicinesBought { get; set; }
    }
}