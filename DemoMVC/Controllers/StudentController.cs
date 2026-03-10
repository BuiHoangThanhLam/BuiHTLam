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
                _context.Student.Add(std);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(std);
        }
        public IActionResult Edit(int Id)
        {
            var student = _context.Student.Find(Id);
            return View(student);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Student std)
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
  
    }
}