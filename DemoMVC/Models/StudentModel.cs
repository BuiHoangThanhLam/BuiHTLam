using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoMVC.Models
{
    [Table("Student")]
    public class Student()
    {
        [Key]
        public string StudentCode {get; set;} = default!;
        public string? FullName {get; set; }
    }
}