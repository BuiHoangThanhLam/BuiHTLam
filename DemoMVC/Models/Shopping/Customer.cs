using System.ComponentModel.DataAnnotations;

namespace DemoMVC.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Tên khách hàng không được để trống")]
        [StringLength(100, ErrorMessage = "Tên tối đa 100 ký tự")]
        [Display(Name = "Tên khách hàng")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [StringLength(15)]
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}