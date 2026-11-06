using MCP_Project.Models;
using MCP_Project.Services;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;

namespace MCP_Project.Tools;

[McpServerToolType]
public class ContractTools
{
    private readonly SpreadsheetService _spreadsheetService;
    private readonly AuditService _auditService;
    private readonly ILogger<ContractTools> _logger;
    public ContractTools(
        SpreadsheetService spreadsheetService,
        AuditService auditService,
        ILogger<ContractTools> logger)
    {
        _spreadsheetService = spreadsheetService;
        _auditService = auditService;
        _logger = logger;
    }

    [McpServerTool]
    public string SearchContracts(string supplier)
    {
        try 
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching contracts for supplier {Supplier}", supplier);
            return $"An error occurred while searching for contracts: {ex.Message}";
        }
    }

    [McpServerTool]
    public string GetContractsByManager(string managerName)
    {
        try
        {
            _logger.LogInformation(
                "Tool executed: GetContractsByManager. Manager={Manager}",
                managerName);

            var contracts =
                _spreadsheetService.GetContractsByManager(managerName);

            _logger.LogInformation(
                "GetContractsByManager returned {Count} contracts",
                contracts.Count);

            if (!contracts.Any())
            {
                return $"No contracts found for manager '{managerName}'.";
            }

            return string.Join(
                Environment.NewLine,
                contracts.Select(c =>
                    $"{c.ContractTitle} | {c.Supplier}"));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error executing GetContractsByManager for {Manager}",
                managerName);

            return "An error occurred while retrieving contracts.";
        }
    }

    [McpServerTool]
    public async Task<string> CountContractsByManager(string managerName)
    {
        try
        {
            _logger.LogInformation(
                "Tool executed: CountContractsByManager. Manager={Manager}",
                managerName);

            var count =
                _spreadsheetService
                    .GetContractsByManager(managerName)
                    .Count;

            var response =
                $"{managerName} is the nominated contract manager for {count} contracts.";

            _logger.LogInformation(
                "CountContractsByManager returned {Count}",
                count);

            await _auditService.LogAsync( new AuditLog
            {
                TimestampUtc = DateTime.UtcNow,
                ToolName = "CountContractsByManager",
                RequestData = managerName,
                ResponseSummary = $"{count} contracts found",
                Success = true
            });
               
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error executing CountContractsByManager for {Manager}",
                managerName);

            await _auditService.LogAsync(new AuditLog
            {
                TimestampUtc = DateTime.UtcNow,
                ToolName = "CountContractsByManager",
                RequestData = managerName,
                Success = false,
                ErrorMessage = ex.Message
            });

            return "An error occurred while counting contracts.";
        }
    }
    //public string CountContractsByManager(string managerName)
    //{
    //    try
    //    {
    //        _logger.LogInformation(
    //            "Tool executed: CountContractsByManager. Manager={Manager}",
    //            managerName);

    //        var count =
    //            _spreadsheetService
    //                .GetContractsByManager(managerName)
    //                .Count;



    //        _logger.LogInformation(
    //            "CountContractsByManager returned {Count}",
    //            count);

    //        return $"{managerName} is the nominated contract manager for {count} contracts.";
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(
    //            ex,
    //            "Error executing CountContractsByManager for {Manager}",
    //            managerName);

    //        return "An error occurred while counting contracts.";
    //    }
    //}
}
