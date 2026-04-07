using Microsoft.AspNetCore.Mvc;
using DemoMVC.Models;
using DemoMVC.Data;
using Microsoft.EntityFrameworkCore;


namespace DemoMVC.Controllers 
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var students = _context.Student.ToList();
            return View(students);
        }
        public IActionResult Create()
        {
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
            return View(std);
        }
        public IActionResult Edit(int Id)
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
        public IActionResult Edit(int Id, Student std)
        {
            if (ModelState.IsValid)
            {
                _context.Student.Update(std);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
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