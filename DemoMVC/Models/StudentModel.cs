using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DemoMVC.Models.Entities;


namespace DemoMVC.Models
{
    [Table("Student")]
    public class Student()
    {
        [Key]
        public int Id { get; set; } = default!;
        [Required(ErrorMessage = "Student Code is required")]
        [StringLength(10, ErrorMessage = "Student Code cannot exceed 10 characters")]
        public string StudentCode { get; set;} = default!;
        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(50, ErrorMessage = "Full Name cannot exceed 50 characters")]
        public string? FullName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Email must follow format abc@abc.abc")]
        public string Email { get; set; }
        [Range(18, 60, ErrorMessage = "Age must be between 18 and 60")]
        public int Age { get; set; }
        public string FacultyId { get; set; } = default!;
        [ForeignKey("FacultyId")]
        public virtual Faculty? Faculty { get; set; } = default!;
    }
}