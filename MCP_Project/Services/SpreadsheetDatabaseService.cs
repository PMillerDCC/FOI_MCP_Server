using ClosedXML.Excel;
using MCP_Project.Models.Spreadsheet;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace MCP_Project.Services;

/// <summary>
/// Provides generic worksheet schema discovery and search capabilities.
/// </summary>
/// <remarks>
/// This implementation intentionally uses a simple spreadsheet contract:
/// row 1 contains headers and row 2 onward contains data.
/// </remarks>
public sealed class SpreadsheetDatabaseService
{
    private readonly IWebHostEnvironment _environment;
    private readonly string _filePath;

    /// <summary>
    /// Initializes a new <see cref="SpreadsheetDatabaseService"/>.
    /// </summary>
    /// <param name="environment">Hosting environment used to locate the workbook in the app content root.</param>
    public SpreadsheetDatabaseService(IWebHostEnvironment environment)
    {
        _environment = environment;

        _filePath = Path.Combine(
        _environment.ContentRootPath,
        "Data",
        "Contracts Register - ANONYMISED.xlsm");

        // Startup path output helps confirm packaged workbook resolution during local development.
        Console.WriteLine(_filePath);
    }

    /// <summary>
    /// Discovers worksheet schema using row 1 as the canonical header row.
    /// </summary>
    /// <returns>Workbook and worksheet schema metadata.</returns>
    public SpreadsheetSchema DiscoverSchema()
    {
        EnsureWorkbookExists();

        using var workbook = new XLWorkbook(_filePath);

        var sheets = new List<SpreadsheetInfo>();

        foreach (var worksheet in workbook.Worksheets)
        {
            // Business rule: the first row is always treated as headers.
            var headerRow = worksheet.Row(1);
            var columns = headerRow
                .CellsUsed()
                .Select(cell => new SpreadsheetColumnInfo
                {
                    ColumnNumber = cell.Address.ColumnNumber,
                    ColumnLetter = cell.Address.ColumnLetter,
                    Name = GetHeaderName(cell)
                })
                .ToList();

            var lastUsedRow = worksheet.LastRowUsed();

            // Data row count excludes the fixed header row.
            var dataRowCount = lastUsedRow is null
                ? 0
                : Math.Max(0, lastUsedRow.RowNumber() - 1);

            sheets.Add(new SpreadsheetInfo
            {
                SheetName = worksheet.Name,
                HeaderRowNumber = 1,
                DataRowCount = dataRowCount,
                Columns = columns
            });
        }

        return new SpreadsheetSchema
        {
            WorkbookName = Path.GetFileName(_filePath),
            Sheets = sheets
        };
    }

    /// <summary>
    /// Searches a single worksheet column for matches.
    /// </summary>
    /// <param name="sheetName">Worksheet name.</param>
    /// <param name="columnName">Header name from row 1.</param>
    /// <param name="searchValue">Value to search for.</param>
    /// <param name="searchOperator">Comparison operator.</param>
    /// <param name="maximumResults">Maximum number of rows to return.</param>
    /// <returns>Matching row payloads.</returns>
    public IReadOnlyList<SpreadsheetSearchResult> Search(
        string sheetName,
        string columnName,
        string searchValue,
        SpreadsheetSearchOperator searchOperator =
            SpreadsheetSearchOperator.Contains,
        int maximumResults = 100)
    {
        ValidateSearchInput(
            sheetName,
            columnName,
            searchValue,
            maximumResults);

        EnsureWorkbookExists();

        using var workbook = new XLWorkbook(_filePath);

        var worksheet = FindWorksheet(workbook, sheetName);

        var headerRow = worksheet.Row(1);

        var headers = BuildHeaderMap(headerRow);

        if (headers.Count == 0)
        {
            throw new InvalidOperationException(
                $"Worksheet '{worksheet.Name}' has no headers in row 1.");
        }

        var searchColumn = headers
            .FirstOrDefault(h =>
                string.Equals(
                    h.Value,
                    columnName,
                    StringComparison.OrdinalIgnoreCase));

        if (searchColumn.Equals(default(
            KeyValuePair<int, string>)))
        {
            var availableColumns = string.Join(
                ", ",
                headers.Values);

            throw new ArgumentException(
                $"Column '{columnName}' was not found in worksheet " +
                $"'{worksheet.Name}'. Available columns: " +
                availableColumns);
        }

        var results = new List<SpreadsheetSearchResult>();

        // Data rows begin at row 2 because row 1 is reserved for headers.
        foreach (var row in worksheet.RowsUsed()
                     .Where(r =>
                         r.RowNumber() >= 2))
        {
            var candidateValue =
                row.Cell(searchColumn.Key).GetFormattedString();

            if (!IsMatch(
                    candidateValue,
                    searchValue,
                    searchOperator))
            {
                continue;
            }

            results.Add(MapRow(
                worksheet.Name,
                row,
                headers));

            if (results.Count >= maximumResults)
            {
                break;
            }
        }

        return results;
    }

    /// <summary>
    /// Searches all worksheets and all used columns for a value.
    /// </summary>
    /// <param name="searchValue">Value to search for.</param>
    /// <param name="maximumResults">Maximum number of rows to return.</param>
    /// <returns>Matching row payloads across workbook sheets.</returns>
    public IReadOnlyList<SpreadsheetSearchResult> SearchAll(
    string searchValue,
    int maximumResults = 100)
    {
        if (string.IsNullOrWhiteSpace(searchValue))
        {
            throw new ArgumentException(
                "A search value is required.",
                nameof(searchValue));
        }

        if (maximumResults is < 1 or > 500)
        {
            throw new ArgumentOutOfRangeException(
                nameof(maximumResults),
                "Maximum results must be between 1 and 500.");
        }

        EnsureWorkbookExists();

        using var workbook = new XLWorkbook(_filePath);

        var results = new List<SpreadsheetSearchResult>();

        foreach (var worksheet in workbook.Worksheets)
        {
            var headerRow = worksheet.Row(1);

            var headers = BuildHeaderMap(headerRow);

            if (headers.Count == 0)
            {
                continue;
            }

            // Row 1 is reserved for headers and excluded from search results.
            foreach (var row in worksheet.RowsUsed()
                         .Where(r =>
                             r.RowNumber() >= 2))
            {
                var found = headers.Keys.Any(columnNumber =>
                    row.Cell(columnNumber)
                        .GetFormattedString()
                        .Contains(
                            searchValue,
                            StringComparison.OrdinalIgnoreCase));

                if (!found)
                {
                    continue;
                }

                results.Add(MapRow(
                    worksheet.Name,
                    row,
                    headers));

                if (results.Count >= maximumResults)
                {
                    return results;
                }
            }
        }

        return results;
    }

    private static IXLWorksheet FindWorksheet(
        XLWorkbook workbook,
        string requestedName)
    {
        var worksheet = workbook.Worksheets
            .FirstOrDefault(sheet =>
                string.Equals(
                    sheet.Name,
                    requestedName,
                    StringComparison.OrdinalIgnoreCase));

        if (worksheet is not null)
        {
            return worksheet;
        }

        var availableSheets = string.Join(
            ", ",
            workbook.Worksheets.Select(s => s.Name));

        throw new ArgumentException(
            $"Worksheet '{requestedName}' was not found. " +
            $"Available worksheets: {availableSheets}");
    }

    private static Dictionary<int, string> BuildHeaderMap(
        IXLRow headerRow)
    {
        var headers = new Dictionary<int, string>();
        var seenHeaders =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        foreach (var cell in headerRow.CellsUsed())
        {
            var header = GetHeaderName(cell);

            // Unique header names are required to build deterministic key/value maps.
            if (!seenHeaders.Add(header))
            {
                throw new InvalidOperationException(
                    $"Duplicate header '{header}' found in row 1. " +
                    "Header names must be unique.");
            }

            headers[cell.Address.ColumnNumber] = header;
        }

        return headers;
    }

    private static SpreadsheetSearchResult MapRow(
        string sheetName,
        IXLRow row,
        IReadOnlyDictionary<int, string> headers)
    {
        var values = new Dictionary<string, string>(
            StringComparer.OrdinalIgnoreCase);

        foreach (var header in headers)
        {
            values[header.Value] =
                row.Cell(header.Key).GetFormattedString();
        }

        return new SpreadsheetSearchResult
        {
            SheetName = sheetName,
            RowNumber = row.RowNumber(),
            Values = values
        };
    }

    private static bool IsMatch(
        string candidate,
        string searchValue,
        SpreadsheetSearchOperator searchOperator)
    {
        return searchOperator switch
        {
            SpreadsheetSearchOperator.Equals =>
                candidate.Equals(
                    searchValue,
                    StringComparison.OrdinalIgnoreCase),

            SpreadsheetSearchOperator.StartsWith =>
                candidate.StartsWith(
                    searchValue,
                    StringComparison.OrdinalIgnoreCase),

            SpreadsheetSearchOperator.EndsWith =>
                candidate.EndsWith(
                    searchValue,
                    StringComparison.OrdinalIgnoreCase),

            _ => candidate.Contains(
                searchValue,
                StringComparison.OrdinalIgnoreCase)
        };
    }

    private static string GetHeaderName(IXLCell cell)
    {
        var header = cell.GetString().Trim();

        return string.IsNullOrWhiteSpace(header)
            ? $"Column_{cell.Address.ColumnLetter}"
            : header;
    }

    private void EnsureWorkbookExists()
    {
        if (!File.Exists(_filePath))
        {
            throw new FileNotFoundException(
                "The spreadsheet could not be found.",
                _filePath);
        }
    }

    private static void ValidateSearchInput(
        string sheetName,
        string columnName,
        string searchValue,
        int maximumResults)
    {
        if (string.IsNullOrWhiteSpace(sheetName))
        {
            throw new ArgumentException(
                "A worksheet name is required.",
                nameof(sheetName));
        }

        if (string.IsNullOrWhiteSpace(columnName))
        {
            throw new ArgumentException(
                "A column name is required.",
                nameof(columnName));
        }

        if (string.IsNullOrWhiteSpace(searchValue))
        {
            throw new ArgumentException(
                "A search value is required.",
                nameof(searchValue));
        }

        if (maximumResults is < 1 or > 500)
        {
            throw new ArgumentOutOfRangeException(
                nameof(maximumResults),
                "Maximum results must be between 1 and 500.");
        }
    }
}
