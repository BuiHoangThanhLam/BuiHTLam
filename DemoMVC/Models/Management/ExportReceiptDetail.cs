using System.ComponentModel.DataAnnotations.Schema;

namespace DemoMVC.Models
{
    public class ExportReceiptDetail
    {
        public int ExportReceiptDetailId { get; set; }

        public int ExportReceiptId { get; set; }
        public int DeviceId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public ExportReceipt? ExportReceipt { get; set; }
        public Device? Device { get; set; }
    }
}