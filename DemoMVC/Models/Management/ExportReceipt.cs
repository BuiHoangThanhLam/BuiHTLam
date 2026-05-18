using System.ComponentModel.DataAnnotations;

namespace DemoMVC.Models
{
    public class ExportReceipt
    {
        public int ExportReceiptId { get; set; }

        [Required]
        [StringLength(50)]
        public string ReceiptCode { get; set; } = string.Empty;

        public DateTime ExportDate { get; set; } = DateTime.Now;

        [StringLength(300)]
        public string? Note { get; set; }

        public ICollection<ExportReceiptDetail> Details { get; set; } = new List<ExportReceiptDetail>();
    }
}