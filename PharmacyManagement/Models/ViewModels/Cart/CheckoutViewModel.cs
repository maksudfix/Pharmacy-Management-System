using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PharmacyManagement.Models.ViewModels.Cart
{
    public class CheckoutViewModel
    {
        [Required]
        public int CustomerId { get; set; }

        public int? PrescriptionId { get; set; }

        [Required(ErrorMessage = "Payment method is required.")]
        [StringLength(30)]
        public string PaymentMethod { get; set; } = string.Empty;

        [Required(ErrorMessage = "Delivery method is required.")]
        [StringLength(30)]
        public string DeliveryMethod { get; set; } = string.Empty;

        [StringLength(300)]
        public string? DeliveryAddress { get; set; }

        [Required]
        public List<CartItemViewModel> CartItems { get; set; } = new();

        public IEnumerable<SelectListItem>? Customers { get; set; }
        public IEnumerable<SelectListItem>? Prescriptions { get; set; }
    }
}