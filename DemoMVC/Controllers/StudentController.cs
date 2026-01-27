using Microsoft.AspNetCore.Mvc;
using DemoMVC.Models;

namespace DemoMVC.Controllers 
{
    public class StudentController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Student std)
        {
            // Nhận đối tượng Student trực tiếp từ Form
            ViewBag.Info = $"Mã SV: {std.StudentCode} - Tên: {std.FullName}";
            return View(std);
        }
    }
}