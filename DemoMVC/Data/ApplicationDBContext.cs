using Microsoft.EntityFrameworkCore;
using DemoMVC.Models;
using DemoMVC.Models.Entities;

namespace DemoMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {}
        public DbSet<Student> Student { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public IEnumerable<object> Students { get; internal set; }
    }
    
    
}