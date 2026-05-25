using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DemoMVC.Models.Entities;


namespace DemoMVC.Models
{
    [Table("HocSinh")]
    public class HocSinh()
    {
        [Key]
        public int Id { get; set; } = default!;
        [Required(ErrorMessage = "Vui lòng nhập đầy đủ thông tin")]
        [StringLength(10, ErrorMessage = "Mã sinh viên không được quá 10 kí tự")]
        public string StudentCode { get; set;} = default!;
        [Required(ErrorMessage = "Vui lòng nhập đầy đủ thông tin")]
        [StringLength(50, ErrorMessage = "Họ và tên không quá 50 kí tự")]
        public string? FullName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập đầy đủ thông tin")]
        [EmailAddress]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Email phải nhập theo định dạng abc@abc.abc")]
        public string Email { get; set; }
        [Range(18, 60, ErrorMessage = "Tuổi phải từ 18 đến 60")]
        public int Age { get; set; }
    }
}