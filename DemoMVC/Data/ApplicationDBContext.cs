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
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<DeviceCategory> DeviceCategories { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<ImportReceipt> ImportReceipts { get; set; }
        public DbSet<ImportReceiptDetail> ImportReceiptDetails { get; set; }
        public DbSet<ExportReceipt> ExportReceipts { get; set; }
        public DbSet<ExportReceiptDetail> ExportReceiptDetails { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<HocSinh> HocSinh { get; set; }
    }
    
    
}