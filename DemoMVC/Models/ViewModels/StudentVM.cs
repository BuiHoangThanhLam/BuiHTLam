namespace DemoMVC.Models.ViewModels
{
    public class StudentVM
    {
        public int Id { get; set; } = default!;
        public string StudentCode { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string Email { get; set; }
        public int Age { get; set; }
        public int FacultyId { get; set; } 
        public string FacultyName { get; set; } = default!;
    }
}