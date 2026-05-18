using System.ComponentModel.DataAnnotations;

namespace DemoMVC.Models.ViewModels
{
    public class ExportReceiptCreateVM
    {
        [Required]
        public string ReceiptCode { get; set; } = string.Empty;

        [Required]
        public DateTime ExportDate { get; set; } = DateTime.Now;

        public string? Note { get; set; }

        public List<ExportReceiptDetailVM> Details { get; set; } = new();
    }

    public class ExportReceiptDetailVM
    {
        public int DeviceId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}