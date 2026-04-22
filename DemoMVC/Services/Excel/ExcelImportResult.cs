namespace DemoMVC.Services.Excel
{
    public class ExcelImportResult
    {
        public int TotalRows { get; set; }
        public int SuccessRows { get; set; }
        public List<string> Errors { get; set; } = new();

        public int FailedRows => TotalRows - SuccessRows;
        public bool HasErrors => Errors.Any();
    }
}