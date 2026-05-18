using System.ComponentModel.DataAnnotations.Schema;

namespace DemoMVC.Models
{
    public class ImportReceiptDetail
    {
        public int ImportReceiptDetailId { get; set; }

        public int ImportReceiptId { get; set; }
        public int DeviceId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public ImportReceipt? ImportReceipt { get; set; }
        public Device? Device { get; set; }
    }
}