using Microsoft.AspNetCore.Identity;

namespace PharmacyManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }
    }
}