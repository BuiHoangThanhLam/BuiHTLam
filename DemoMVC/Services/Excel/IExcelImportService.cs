using Microsoft.AspNetCore.Http;

namespace DemoMVC.Services.Excel
{
    public interface IExcelImportService
    {
        Task<ExcelImportResult> ImportAsync<T>(
            IFormFile file,
            ExcelImportOptions? options = null,
            CancellationToken cancellationToken = default
        ) where T : class, new();
    }
}