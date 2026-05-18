using System.ComponentModel.DataAnnotations;

namespace DemoMVC.Models.ViewModels
{
    public class ImportReceiptCreateVM
    {
        [Required]
        public string ReceiptCode { get; set; } = string.Empty;

        [Required]
        public DateTime ImportDate { get; set; } = DateTime.Now;

        public string? Note { get; set; }

        public List<ImportReceiptDetailVM> Details { get; set; } = new();
    }

    public class ImportReceiptDetailVM
    {
        public int DeviceId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}