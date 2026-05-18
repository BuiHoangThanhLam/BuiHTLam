using DemoMVC.Data;
using DemoMVC.Models;
using DemoMVC.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DemoMVC.Controllers
{
    public class ImportReceiptsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ImportReceiptsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.ImportReceipts
                .OrderByDescending(x => x.ImportDate)
                .ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.Devices = new SelectList(_context.Devices, "DeviceId", "DeviceName");

            var vm = new ImportReceiptCreateVM();
            vm.Details.Add(new ImportReceiptDetailVM());

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ImportReceiptCreateVM vm)
        {
            if (_context.ImportReceipts.Any(x => x.ReceiptCode == vm.ReceiptCode))
            {
                ModelState.AddModelError("ReceiptCode", "Mã phiếu nhập đã tồn tại");
            }

            if (vm.Details == null || !vm.Details.Any())
            {
                ModelState.AddModelError("", "Phiếu nhập phải có ít nhất 1 dòng chi tiết");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Devices = new SelectList(_context.Devices, "DeviceId", "DeviceName");
                return View(vm);
            }

            var receipt = new ImportReceipt
            {
                ReceiptCode = vm.ReceiptCode,
                ImportDate = vm.ImportDate,
                Note = vm.Note
            };

            _context.ImportReceipts.Add(receipt);
            await _context.SaveChangesAsync();

            foreach (var item in vm.Details)
            {
                var detail = new ImportReceiptDetail
                {
                    ImportReceiptId = receipt.ImportReceiptId,
                    DeviceId = item.DeviceId,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity,
                    Amount = item.UnitPrice * item.Quantity
                };

                _context.ImportReceiptDetails.Add(detail);

                var device = await _context.Devices.FindAsync(item.DeviceId);
                if (device != null)
                {
                    device.QuantityInStock += item.Quantity;
                    device.ImportPrice = item.UnitPrice;
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var receipt = await _context.ImportReceipts
                .Include(x => x.Details)
                .ThenInclude(x => x.Device)
                .FirstOrDefaultAsync(x => x.ImportReceiptId == id);

            if (receipt == null) return NotFound();

            return View(receipt);
        }
    }
}