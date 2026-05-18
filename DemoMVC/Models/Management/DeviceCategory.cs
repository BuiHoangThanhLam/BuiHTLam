using System.ComponentModel.DataAnnotations;

namespace DemoMVC.Models
{
    public class DeviceCategory
    { 
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        [StringLength(300)]
        public string? Description { get; set; }

        public ICollection<Device> Devices { get; set; } = new List<Device>();
    }
}