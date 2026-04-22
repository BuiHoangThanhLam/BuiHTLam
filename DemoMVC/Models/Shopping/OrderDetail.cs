using System.ComponentModel.DataAnnotations;

namespace DemoMVC.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn đơn hàng")]
        [Display(Name = "Đơn hàng")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn sản phẩm")]
        [Display(Name = "Sản phẩm")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Số lượng không được để trống")]
        [Range(1, 1000, ErrorMessage = "Số lượng phải lớn hơn 0")]
        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Đơn giá không được để trống")]
        [Range(0.01, 999999999, ErrorMessage = "Đơn giá phải lớn hơn 0")]
        [Display(Name = "Đơn giá")]
        public decimal UnitPrice { get; set; }

        public Order? Order { get; set; }
        public Product? Product { get; set; }
    }
}