using Microsoft.AspNetCore.Mvc;
using DemoMVC.Models;
using DemoMVC.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using DemoMVC.Models.ViewModels;
using ClosedXML.Excel;
using DemoMVC.Services.Excel;


namespace DemoMVC.Controllers 
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IExcelImportService _excelImportService;

        public StudentController(
            ApplicationDbContext context,
            IExcelImportService excelImportService)
        {
            _context = context;
            _excelImportService = excelImportService;
        }
        [HttpGet]
        public IActionResult ImportExcel()
        {
            return View(new ExcelUploadViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportExcel(
            ExcelUploadViewModel model,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _excelImportService.ImportAsync<Student>(
                model.ExcelFile,
                new ExcelImportOptions
                {
                    RequiredColumns = new List<string> { "StudentCode", "FullName", "Email", "Age", "FacultyId"},
                    BatchSize = 200
                },
                cancellationToken);

            TempData["Success"] = $"Import thành công {result.SuccessRows}/{result.TotalRows} dòng.";

            if (result.HasErrors)
            {
                TempData["Error"] = string.Join("<br/>", result.Errors.Take(20));
            }

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Index()
        {
            var result = await _context.Student
                            .Select(s => new StudentVM
                            {
                                Id = s.Id,
                                StudentCode = s.StudentCode,
                                FullName = s.FullName,
                                Email = s.Email,
                                Age = s.Age,
                                FacultyName = s.Faculty!.FacultyName
                            })
                            .ToListAsync();
            return View(result);
        }
        public IActionResult Create()
        {
            ViewData["FacultyId"] = new SelectList(_context.Faculties, "FacultyId", "FacultyName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student std)
        {
            if (ModelState.IsValid)
            {
                 if (_context.Student.Any(s => s.StudentCode == std.StudentCode))
                {
                    ModelState.AddModelError("StudentCode", "Student Code already exists");
                    return View(std);
                }
                _context.Student.Add(std);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FacultyId"] = new SelectList(_context.Faculties, "FacultyId", "FacultyName", std.FacultyId);
            return View(std);
        }

        private object IndexAsync()
        {
            throw new NotImplementedException();
        }

        public IActionResult Edit(int Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var student = _context.Student.Find(Id);
            if (student == null)
            {
                return RedirectToAction("NotFoundPage");
            }
            ViewData["FacultyId"] = new SelectList(_context.Faculties, "FacultyId", "FacultyName", student.FacultyId);
            return View(student);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int Id, Student std)
        {
            if (ModelState.IsValid)
            {
                _context.Student.Update(std);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["FacultyId"] = new SelectList(_context.Faculties, "FacultyId", "FacultyName", std.FacultyId);
            return View(std);
        }
        public IActionResult Delete(int Id)
        {
            var student = _context.Student.Find(Id);
            if (student == null)
            {
                return RedirectToAction("NotFoundPage");
            }
            return View(student);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int Id)
        {
            var student = _context.Student.Find(Id);
            if (student != null)
            {
                _context.Student.Remove(student);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public IActionResult NotFoundPage()      
        {
            return View("NotFound");
        }
 
    }
}