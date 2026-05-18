using System.ComponentModel.DataAnnotations;

namespace DemoMVC.Models
{
    public class Supplier
    {
        public int SupplierId { get; set; }

        [Required]
        [StringLength(100)]
        public string SupplierName { get; set; } = string.Empty;

        [Phone]
        public string? Phone { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        public ICollection<Device> Devices { get; set; } = new List<Device>();
    }
}