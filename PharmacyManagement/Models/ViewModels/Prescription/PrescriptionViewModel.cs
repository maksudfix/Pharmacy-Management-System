namespace PharmacyManagement.Models.ViewModels.Prescription
{
    public class PrescriptionViewModel
    {
        public int PrescriptionId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
    }
}