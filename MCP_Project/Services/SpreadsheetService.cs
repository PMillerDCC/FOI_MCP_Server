using ClosedXML.Excel;
using MCP_Project.Models;

namespace MCP_Project.Services;

/// <summary>
/// Provides contract-specific spreadsheet parsing and filtering operations.
/// </summary>
public class SpreadsheetService
{
    private readonly string _filePath;

    /// <summary>
    /// Initializes a new <see cref="SpreadsheetService"/>.
    /// </summary>
    /// <remarks>
    /// This service currently uses a fixed local workbook path because it is designed
    /// as a proof-of-concept for a known source file.
    /// </remarks>
    public SpreadsheetService()
    {
        _filePath =
            @"C:\Users\philip.miller\OneDrive - Durham County Council\Apprenticeship\QA\Major Project\Assignment\FOI Data\Contracts\Contracts Register - ANONYMISED.xlsm";
    }

    /// <summary>
    /// Reads contract rows from the Frameworks worksheet and maps them to contract records.
    /// </summary>
    /// <returns>All parsed framework contracts.</returns>
    public List<ContractRecord> GetFrameworkContracts()
    {
        var contracts = new List<ContractRecord>();

        using var workbook = new XLWorkbook(_filePath);

        var worksheet = workbook.Worksheet("Frameworks");

        // Business rule: row 1 is the header row; data begins from row 2.
        var rows = worksheet.RowsUsed().Skip(1);

        foreach (var row in rows)
        {
            contracts.Add(new ContractRecord
            {
                MasterRefNo = row.Cell(1).GetString(),
                ContractReference = row.Cell(2).GetString(),
                ClassReferenceNumber = row.Cell(3).GetString(),
                Classification = row.Cell(4).GetString(),
                ServiceArea = row.Cell(5).GetString(),
                ContractManager = row.Cell(7).GetString(),
                ContractManagerEmail = row.Cell(8).GetString(),
                ProcurementOfficer = row.Cell(9).GetString(),
                FrameworksTitle = row.Cell(10).GetString(),
                Supplier = row.Cell(12).GetString(),
                // The source workbook stores IT relevance as Yes/No text.
                RelevantToIT =
                            row.Cell(11)
                            .GetString()
                            .Equals("Yes",
                                StringComparison.OrdinalIgnoreCase),
                ContractStartDate = TryGetDate(row.Cell(15)),
                ContractEndDate = TryGetDate(row.Cell(16)),
                TotalContractValue = TryGetDecimal(row.Cell(18))
            });
        }

        return contracts;
    }
    private static DateTime? TryGetDate(IXLCell cell)
    {
        if (cell.TryGetValue<DateTime>(out var date))
        {
            return date;
        }

        return null;
    }
    private static decimal? TryGetDecimal(IXLCell cell)
    {
        if (cell.TryGetValue<decimal>(out var value))
        {
            return value;
        }

        return null;
    }
    /// <summary>
    /// Searches known contract text fields for a supplied value.
    /// </summary>
    /// <param name="searchText">Text fragment to match.</param>
    /// <returns>Contracts matching any supported field.</returns>
    public List<ContractRecord> SearchContracts(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return [];
        }

        return GetFrameworkContracts()
        .Where(contract =>
            ContainsText(contract.ContractReference, searchText) ||
            ContainsText(contract.ClassReferenceNumber, searchText) ||
            ContainsText(contract.Classification, searchText) ||
            ContainsText(contract.ServiceArea, searchText) ||
            ContainsText(contract.ContractManager, searchText) ||
            ContainsText(contract.ContractManagerEmail, searchText) ||
            ContainsText(contract.ProcurementOfficer, searchText) ||
            ContainsText(contract.FrameworksTitle, searchText) ||
            ContainsText(contract.Supplier, searchText)) 
        .ToList();
    }

    private static bool ContainsText(
        string? value,
        string searchText)
    { 
        return !string.IsNullOrWhiteSpace(value) &&
               value.Contains(
                   searchText, 
                   StringComparison.OrdinalIgnoreCase);
    }
}

