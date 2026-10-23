using ModelContextProtocol.Server;
using MCP_Project.Services;

namespace MCP_Project.Tools;

[McpServerToolType]
public class ContractTools
{
    private readonly SpreadsheetService _spreadsheetService;

    public ContractTools(
        SpreadsheetService spreadsheetService)
    {
        _spreadsheetService = spreadsheetService;
    }

    [McpServerTool]
    public string SearchContracts(string supplier)
    {
        var results =
            _spreadsheetService.SearchBySupplier(supplier);

        if (!results.Any())
        {
            return $"No contracts found for '{supplier}'.";
        }

        return string.Join(
            Environment.NewLine,
            results.Select(c =>
                $"""
                Supplier: {c.Supplier}
                Title: {c.ContractTitle}
                Service Area: {c.ServiceArea}
                Value: £{c.TotalContractValue:N2}
                End Date: {c.ContractEndDate:dd/MM/yyyy}
                """
            ));
    }

    [McpServerTool]
    public string GetContractsByManager(string managerName)
    {
        var contracts =
            _spreadsheetService.GetContractsByManager(managerName);
        if (!contracts.Any())
        { 
            return $"No contracts found for manager '{managerName}'.";
        }

        return string.Join(
            Environment.NewLine,
            contracts.Select(c =>
              $"{c.ContractTitle} | {c.Supplier}"));
    }

    [McpServerTool]
    public string CountContractsByManager(string managerName)
    { 
        var count = 
            _spreadsheetService
                .GetContractsByManager(managerName)
                .Count;
        return $"{managerName} is the nominated contract manager for {count} contracts.";
    }
}
