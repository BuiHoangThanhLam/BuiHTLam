using DemoMVC.Data;
using DemoMVC.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DemoMVC.ViewModels;
using DemoMVC.Models;
namespace DemoMVC.Controllers
{
    public class HocSinhController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetHocSinhs(int page = 1, int pageSize = 10)
        {
            var query = _context.HocSinh
                .AsNoTracking()
                .OrderByDescending(x => x.Id);

            var totalItems = await query.CountAsync();

            var hocSinhs = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new PagedResult<HocSinh>
            {
                Items = hocSinhs,
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            return PartialView("HocSinhTable", result);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("CreateHocSinh");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HocSinh hocsinh)
        {
            hocsinh.Id = 0;

            // StudentCode tự sinh, không lấy từ form
            hocsinh.StudentCode = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            // Bỏ validate StudentCode vì không nhập từ form
            ModelState.Remove(nameof(HocSinh.StudentCode));

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value != null && x.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value!.Errors.Select(e => $"{x.Key}: {e.ErrorMessage}"))
                    .ToList();

                return Json(new
                {
                    success = false,
                    errors = errors
                });
            }

            try
            {
                _context.HocSinh.Add(hocsinh);

                await _context.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    redirectUrl = Url.Action("Index", "HocSinh")
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    errors = new List<string>
                    {
                        ex.InnerException?.Message ?? ex.Message
                    }
                });
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var hocsinh = await _context.HocSinh.FindAsync(id);

            if (hocsinh == null)
            {
                return NotFound();
            }

            return PartialView("EditHocSinh", hocsinh);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HocSinh hocsinh)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("EditHocSinh", hocsinh);
            }

            var existingHocSinh = await _context.HocSinh.FindAsync(hocsinh.Id);

            if (existingHocSinh == null)
            {
                return NotFound();
            }

            existingHocSinh.StudentCode = hocsinh.StudentCode;
            existingHocSinh.FullName = hocsinh.FullName;
            existingHocSinh.Email = hocsinh.Email;
            existingHocSinh.Age = hocsinh.Age;

            await _context.SaveChangesAsync();
            return Json(new
            {
                success = true
            });
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var hocsinh = await _context.HocSinh
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (hocsinh == null)
            {
                return NotFound();
            }

            return PartialView("DeleteHocSinh", hocsinh);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(HocSinh hocsinh)
        {
            var existingHocSinh = await _context.HocSinh
                .FindAsync(hocsinh.Id);

            if (existingHocSinh == null)
            {
                return Json(new
                {
                    success = false
                });
            }

            _context.HocSinh.Remove(existingHocSinh);

            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true
            });
        }
    }
}