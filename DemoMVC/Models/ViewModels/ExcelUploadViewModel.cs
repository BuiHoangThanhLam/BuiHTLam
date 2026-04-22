using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DemoMVC.Models.ViewModels
{
    public class ExcelUploadViewModel
    {
        [Required(ErrorMessage = "Vui lòng chọn file Excel")]
        public IFormFile ExcelFile { get; set; } = default!;
    }
}