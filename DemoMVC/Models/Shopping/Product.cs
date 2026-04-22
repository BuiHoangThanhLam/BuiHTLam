using System.ComponentModel.DataAnnotations;

namespace DemoMVC.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(100)]
        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Giá không được để trống")]
        [Range(0.01, 999999999, ErrorMessage = "Giá phải lớn hơn 0")]
        [Display(Name = "Giá")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Số lượng tồn không được để trống")]
        [Range(0, 100000, ErrorMessage = "Số lượng tồn phải >= 0")]
        [Display(Name = "Số lượng tồn")]
        public int StockQuantity { get; set; }

        [StringLength(300)]
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}