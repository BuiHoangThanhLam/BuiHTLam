using DemoMVC.Data;
using DemoMVC.Models;
using DemoMVC.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DemoMVC.Controllers
{
    public class ExportReceiptsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExportReceiptsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.ExportReceipts
                .OrderByDescending(x => x.ExportDate)
                .ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.Devices = new SelectList(_context.Devices, "DeviceId", "DeviceName");

            var vm = new ExportReceiptCreateVM();
            vm.Details.Add(new ExportReceiptDetailVM());

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExportReceiptCreateVM vm)
        {
            if (_context.ExportReceipts.Any(x => x.ReceiptCode == vm.ReceiptCode))
            {
                ModelState.AddModelError("ReceiptCode", "Mã phiếu xuất đã tồn tại");
            }

            if (vm.Details == null || !vm.Details.Any())
            {
                ModelState.AddModelError("", "Phiếu xuất phải có ít nhất 1 dòng chi tiết");
            }

            foreach (var item in vm.Details)
            {
                var device = await _context.Devices.FindAsync(item.DeviceId);

                if (device == null)
                {
                    ModelState.AddModelError("", "Thiết bị không tồn tại");
                    continue;
                }

                if (device.QuantityInStock < item.Quantity)
                {
                    ModelState.AddModelError("", $"Thiết bị {device.DeviceName} không đủ tồn kho");
                }
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Devices = new SelectList(_context.Devices, "DeviceId", "DeviceName");
                return View(vm);
            }

            var receipt = new ExportReceipt
            {
                ReceiptCode = vm.ReceiptCode,
                ExportDate = vm.ExportDate,
                Note = vm.Note
            };

            _context.ExportReceipts.Add(receipt);
            await _context.SaveChangesAsync();

            foreach (var item in vm.Details)
            {
                var detail = new ExportReceiptDetail
                {
                    ExportReceiptId = receipt.ExportReceiptId,
                    DeviceId = item.DeviceId,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity,
                    Amount = item.UnitPrice * item.Quantity
                };

                _context.ExportReceiptDetails.Add(detail);

                var device = await _context.Devices.FindAsync(item.DeviceId);
                if (device != null)
                {
                    device.QuantityInStock -= item.Quantity;
                    device.ExportPrice = item.UnitPrice;
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var receipt = await _context.ExportReceipts
                .Include(x => x.Details)
                .ThenInclude(x => x.Device)
                .FirstOrDefaultAsync(x => x.ExportReceiptId == id);

            if (receipt == null) return NotFound();

            return View(receipt);
        }
    }
}