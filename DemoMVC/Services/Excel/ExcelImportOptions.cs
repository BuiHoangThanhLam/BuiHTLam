namespace DemoMVC.Services.Excel
{
    public class ExcelImportOptions
    {
        public int HeaderRowNumber { get; set; } = 1;
        public string? WorksheetName { get; set; }
        public bool SkipEmptyRows { get; set; } = true;
        public bool IgnoreUnknownColumns { get; set; } = true;
        public int BatchSize { get; set; } = 200;

        // Những cột bắt buộc phải có trong file Excel
        public List<string> RequiredColumns { get; set; } = new();
    }
}