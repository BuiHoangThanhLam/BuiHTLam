using Microsoft.AspNetCore.Mvc;

namespace DemoMVC.Controllers 
{
    public class DemoController : Controller
    {
        // URL: /Demo/Index
        public IActionResult Index()
        {
            // 1. Khai báo biến 
            string hoTen = "Bùi Hoàng Thanh Lâm";
            string maSinhVien = "2221050435";

            // 2. Tạo chuỗi thông báo
            string thongBao = $"Hello {hoTen} - {maSinhVien}";

            // 3. Gửi dữ liệu sang View thông qua ViewBag
            ViewBag.ThongBaoHienThi = thongBao;

            return View();
        }
    }
}