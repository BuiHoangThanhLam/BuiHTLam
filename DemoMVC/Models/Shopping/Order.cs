using System.ComponentModel.DataAnnotations;

namespace DemoMVC.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        [Required]
        [Display(Name = "Ngày đặt")]
        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Vui lòng chọn khách hàng")]
        [Display(Name = "Khách hàng")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Trạng thái không được để trống")]
        [StringLength(50)]
        [Display(Name = "Trạng thái")]
        public string Status { get; set; } = "Mới tạo";

        public Customer? Customer { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}