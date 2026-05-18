using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DemoMVC.Data;
using DemoMVC.Models;

namespace DemoMVC.Controllers
{
    public class DeviceCategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DeviceCategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DeviceCategory
        public async Task<IActionResult> Index()
        {
            return View(await _context.DeviceCategories.ToListAsync());
        }

        // GET: DeviceCategory/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deviceCategory = await _context.DeviceCategories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (deviceCategory == null)
            {
                return NotFound();
            }

            return View(deviceCategory);
        }

        // GET: DeviceCategory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DeviceCategory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName,Description")] DeviceCategory deviceCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deviceCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(deviceCategory);
        }

        // GET: DeviceCategory/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deviceCategory = await _context.DeviceCategories.FindAsync(id);
            if (deviceCategory == null)
            {
                return NotFound();
            }
            return View(deviceCategory);
        }

        // POST: DeviceCategory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName,Description")] DeviceCategory deviceCategory)
        {
            if (id != deviceCategory.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deviceCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeviceCategoryExists(deviceCategory.CategoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(deviceCategory);
        }

        // GET: DeviceCategory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deviceCategory = await _context.DeviceCategories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (deviceCategory == null)
            {
                return NotFound();
            }

            return View(deviceCategory);
        }

        // POST: DeviceCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deviceCategory = await _context.DeviceCategories.FindAsync(id);
            if (deviceCategory != null)
            {
                _context.DeviceCategories.Remove(deviceCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeviceCategoryExists(int id)
        {
            return _context.DeviceCategories.Any(e => e.CategoryId == id);
        }
    }
}
