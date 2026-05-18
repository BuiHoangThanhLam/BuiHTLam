using System.ComponentModel.DataAnnotations;

namespace DemoMVC.Models
{
    public class ImportReceipt
    {
        public int ImportReceiptId { get; set; }

        [Required]
        [StringLength(50)]
        public string ReceiptCode { get; set; } = string.Empty;

        public DateTime ImportDate { get; set; } = DateTime.Now;

        [StringLength(300)]
        public string? Note { get; set; }

        public ICollection<ImportReceiptDetail> Details { get; set; } = new List<ImportReceiptDetail>();
    }
}