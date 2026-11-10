using ClosedXML.Excel;
using MCP_Project.Models;

namespace MCP_Project.Services;

public class SpreadsheetService
{
    private readonly string _filePath;

    public SpreadsheetService()
    {
        _filePath =
            @"C:\Users\philip.miller\OneDrive - Durham County Council\Apprenticeship\QA\Major Project\Assignment\FOI Data\Contracts\Contracts Register - ANONYMISED.xlsm";
    }

    public List<ContractRecord> GetFrameworkContracts()
    {
        var contracts = new List<ContractRecord>();

        using var workbook = new XLWorkbook(_filePath);

        var worksheet = workbook.Worksheet("Frameworks");

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

