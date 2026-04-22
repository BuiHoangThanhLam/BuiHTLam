using ClosedXML.Excel;
using DemoMVC.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DemoMVC.Services.Excel
{
    public class ExcelImportService : IExcelImportService
    {
        private readonly ApplicationDbContext _context;

        public ExcelImportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ExcelImportResult> ImportAsync<T>(
            IFormFile file,
            ExcelImportOptions? options = null,
            CancellationToken cancellationToken = default
        ) where T : class, new()
        {
            options ??= new ExcelImportOptions();
            var result = new ExcelImportResult();

            if (file == null || file.Length == 0)
            {
                result.Errors.Add("File không hợp lệ hoặc đang rỗng.");
                return result;
            }

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (extension != ".xlsx")
            {
                result.Errors.Add("Chỉ hỗ trợ file Excel .xlsx");
                return result;
            }

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream, cancellationToken);
            stream.Position = 0;

            using var workbook = new XLWorkbook(stream);

            var worksheet = !string.IsNullOrWhiteSpace(options.WorksheetName)
                ? workbook.Worksheet(options.WorksheetName)
                : workbook.Worksheet(1);

            var headerRow = worksheet.Row(options.HeaderRowNumber);
            var headers = BuildHeaderMap(headerRow);

            if (!headers.Any())
            {
                result.Errors.Add("Không đọc được dòng tiêu đề trong file Excel.");
                return result;
            }

            var missingRequiredColumns = options.RequiredColumns
                .Where(x => !headers.ContainsKey(Normalize(x)))
                .ToList();

            if (missingRequiredColumns.Any())
            {
                result.Errors.Add("Thiếu cột bắt buộc: " + string.Join(", ", missingRequiredColumns));
                return result;
            }

            var propertyMap = GetImportableProperties(typeof(T));

            var lastRowNumber = worksheet.LastRowUsed()?.RowNumber() ?? options.HeaderRowNumber;
            var batch = new List<T>();

            for (int rowNumber = options.HeaderRowNumber + 1; rowNumber <= lastRowNumber; rowNumber++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var row = worksheet.Row(rowNumber);

                if (options.SkipEmptyRows && IsEmptyRow(row, headers.Values.Max()))
                    continue;

                result.TotalRows++;

                var rowErrors = new List<string>();
                var entity = new T();

                foreach (var header in headers)
                {
                    var normalizedHeader = header.Key;
                    var columnIndex = header.Value;

                    if (!propertyMap.TryGetValue(normalizedHeader, out var property))
                    {
                        if (!options.IgnoreUnknownColumns)
                        {
                            rowErrors.Add($"Cột '{header.Key}' không tồn tại trong model.");
                        }
                        continue;
                    }

                    var cell = row.Cell(columnIndex);

                    if (cell.IsEmpty())
                        continue;

                    try
                    {
                        var convertedValue = ConvertCellValue(cell, property.PropertyType);

                        if (convertedValue != null)
                        {
                            property.SetValue(entity, convertedValue);
                        }
                    }
                    catch
                    {
                        rowErrors.Add($"Cột '{property.Name}' sai kiểu dữ liệu.");
                    }
                }

                var validationErrors = ValidateModel(entity);
                rowErrors.AddRange(validationErrors);

                if (rowErrors.Any())
                {
                    foreach (var error in rowErrors)
                    {
                        result.Errors.Add($"Dòng {rowNumber}: {error}");
                    }
                    continue;
                }

                batch.Add(entity);

                if (batch.Count >= options.BatchSize)
                {
                    await SaveBatchAsync(batch, result, cancellationToken);
                    batch.Clear();
                }
            }

            if (batch.Any())
            {
                await SaveBatchAsync(batch, result, cancellationToken);
                batch.Clear();
            }

            return result;
        }

        private async Task SaveBatchAsync<T>(
            List<T> batch,
            ExcelImportResult result,
            CancellationToken cancellationToken
        ) where T : class
        {
            try
            {
                _context.Set<T>().AddRange(batch);
                await _context.SaveChangesAsync(cancellationToken);

                result.SuccessRows += batch.Count;

                // Giảm tracking để đỡ nặng bộ nhớ khi import nhiều dòng
                _context.ChangeTracker.Clear();
            }
            catch (Exception ex)
            {
                result.Errors.Add("Lỗi khi lưu dữ liệu vào database: " + ex.Message);
                _context.ChangeTracker.Clear();
            }
        }

        private static Dictionary<string, int> BuildHeaderMap(IXLRow headerRow)
        {
            var headers = new Dictionary<string, int>();

            var lastCell = headerRow.LastCellUsed()?.Address.ColumnNumber ?? 0;

            for (int col = 1; col <= lastCell; col++)
            {
                var rawHeader = headerRow.Cell(col).GetString().Trim();

                if (string.IsNullOrWhiteSpace(rawHeader))
                    continue;

                var normalizedHeader = Normalize(rawHeader);

                if (!headers.ContainsKey(normalizedHeader))
                {
                    headers.Add(normalizedHeader, col);
                }
            }

            return headers;
        }

        private static Dictionary<string, PropertyInfo> GetImportableProperties(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p =>
                    p.CanWrite &&
                    IsSimpleType(p.PropertyType) &&
                    !string.Equals(p.Name, "Id", StringComparison.OrdinalIgnoreCase))
                .ToDictionary(p => Normalize(p.Name), p => p);
        }

        private static bool IsSimpleType(Type type)
        {
            var actualType = Nullable.GetUnderlyingType(type) ?? type;

            return actualType.IsPrimitive
                   || actualType.IsEnum
                   || actualType == typeof(string)
                   || actualType == typeof(decimal)
                   || actualType == typeof(DateTime)
                   || actualType == typeof(Guid);
        }

        private static object? ConvertCellValue(IXLCell cell, Type targetType)
        {
            var actualType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            if (actualType == typeof(string))
                return cell.GetString().Trim();

            if (actualType == typeof(int))
            {
                if (cell.TryGetValue<int>(out var intValue)) return intValue;
                if (int.TryParse(cell.GetString(), out intValue)) return intValue;
                return null;
            }

            if (actualType == typeof(long))
            {
                if (cell.TryGetValue<long>(out var longValue)) return longValue;
                if (long.TryParse(cell.GetString(), out longValue)) return longValue;
                return null;
            }

            if (actualType == typeof(decimal))
            {
                if (cell.TryGetValue<decimal>(out var decimalValue)) return decimalValue;
                if (decimal.TryParse(cell.GetString(), out decimalValue)) return decimalValue;
                return null;
            }

            if (actualType == typeof(double))
            {
                if (cell.TryGetValue<double>(out var doubleValue)) return doubleValue;
                if (double.TryParse(cell.GetString(), out doubleValue)) return doubleValue;
                return null;
            }

            if (actualType == typeof(bool))
            {
                if (cell.TryGetValue<bool>(out var boolValue)) return boolValue;

                var text = cell.GetString().Trim().ToLower();
                if (text == "true" || text == "1" || text == "yes") return true;
                if (text == "false" || text == "0" || text == "no") return false;

                return null;
            }

            if (actualType == typeof(DateTime))
            {
                if (cell.TryGetValue<DateTime>(out var dateValue)) return dateValue;
                if (DateTime.TryParse(cell.GetString(), out dateValue)) return dateValue;
                return null;
            }

            if (actualType == typeof(Guid))
            {
                if (Guid.TryParse(cell.GetString(), out var guidValue)) return guidValue;
                return null;
            }

            if (actualType.IsEnum)
            {
                var text = cell.GetString().Trim();
                if (Enum.TryParse(actualType, text, true, out var enumValue))
                    return enumValue;
                return null;
            }

            return null;
        }

        private static List<string> ValidateModel<T>(T model)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(model!);

            Validator.TryValidateObject(model!, context, validationResults, true);

            return validationResults
                .Select(x => x.ErrorMessage ?? "Dữ liệu không hợp lệ.")
                .ToList();
        }

        private static bool IsEmptyRow(IXLRow row, int lastColumn)
        {
            for (int col = 1; col <= lastColumn; col++)
            {
                if (!row.Cell(col).IsEmpty())
                    return false;
            }

            return true;
        }

        private static string Normalize(string text)
        {
            return text
                .Trim()
                .Replace(" ", "")
                .Replace("_", "")
                .Replace("-", "")
                .ToLowerInvariant();
        }
    }
}