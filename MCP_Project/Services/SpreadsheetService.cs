using ClosedXML.Excel;
using MCP_Project.Models;

namespace MCP_Project.Services;

public class SpreadsheetService
{
    private readonly string _filePath;

    public SpreadsheetService()
    {
        _filePath =
            @"C:\Users\philip.miller\OneDrive - Durham County Council\Apprenticeship\QA\Major Project\Assignment\FOI Data\Contracts\Contracts Register.xlsm";
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
                ServiceArea = row.Cell(7).GetString(),
                ContractManager = row.Cell(8).GetString(),
                ContractTitle = row.Cell(12).GetString(),
                Supplier = row.Cell(14).GetString(),

                RelevantToIT =
                    row.Cell(13)
                        .GetString()
                        .Equals("Yes",
                            StringComparison.OrdinalIgnoreCase),

                ContractStartDate =
                    TryGetDate(row.Cell(17)),

                ContractEndDate =
                    TryGetDate(row.Cell(18)),

                TotalContractValue =
                    TryGetDecimal(row.Cell(20))
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
    public List<ContractRecord> SearchBySupplier(string supplier)
    {
        return GetFrameworkContracts()
            .Where(c =>
                !string.IsNullOrWhiteSpace(c.Supplier) &&
                c.Supplier.Contains(
                    supplier,
                    StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
    public List<ContractRecord> GetContractsByManager(string managerName)
    { 
        return GetFrameworkContracts()
            .Where(c =>
                !string.IsNullOrWhiteSpace(c.ContractManager) &&
                c.ContractManager.Contains(
                    managerName,
                    StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}

