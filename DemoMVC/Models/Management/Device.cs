using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoMVC.Models
{
    public class Device
    {
        public int DeviceId { get; set; }

        [Required]
        [StringLength(50)]
        public string DeviceCode { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string DeviceName { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int SupplierId { get; set; }

        [StringLength(50)]
        public string? Unit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ImportPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ExportPrice { get; set; }

        public int QuantityInStock { get; set; }

        [StringLength(300)]
        public string? Description { get; set; }

        public DeviceCategory? Category { get; set; }
        public Supplier? Supplier { get; set; }

        public ICollection<ImportReceiptDetail> ImportReceiptDetails { get; set; } = new List<ImportReceiptDetail>();
        public ICollection<ExportReceiptDetail> ExportReceiptDetails { get; set; } = new List<ExportReceiptDetail>();
    }
}