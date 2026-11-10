using System.ComponentModel;
using MCP_Project.Models;
using MCP_Project.Models.Spreadsheet;
using MCP_Project.Services;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;

namespace MCP_Project.Tools;

[McpServerToolType]
public sealed class SpreadsheetTools
{
    private readonly SpreadsheetDatabaseService _spreadsheetDatabaseService;
    private readonly SpreadsheetService _spreadsheetService;
    private readonly AuditService _auditService;
    private readonly ILogger<SpreadsheetTools> _logger;

    public SpreadsheetTools(
        SpreadsheetDatabaseService spreadsheetDatabaseService,
        SpreadsheetService spreadsheetService,
        AuditService auditService,
        ILogger<SpreadsheetTools> logger)
    {
        _spreadsheetDatabaseService = spreadsheetDatabaseService;
        _spreadsheetService = spreadsheetService;
        _auditService = auditService;
        _logger = logger;
    }

    /*
     * Generic spreadsheet discovery and search tools
     */

    [McpServerTool]
    [Description(
        "Discovers the spreadsheet structure, including worksheet names, " +
        "column names and data-row counts. Use this before a targeted " +
        "spreadsheet search when the sheet or column names are unknown.")]
    public async Task<SpreadsheetSchema> DiscoverSpreadsheet()
    {
        const string toolName = nameof(DiscoverSpreadsheet);
        const string requestData = "No parameters";

        try
        {
            _logger.LogInformation(
                "Tool executed: {ToolName}",
                toolName);

            var schema =
                _spreadsheetDatabaseService.DiscoverSchema();

            _logger.LogInformation(
                "{ToolName} completed successfully",
                toolName);

            await WriteAuditLogAsync(
                toolName,
                requestData,
                "Spreadsheet schema discovered successfully",
                true);

            return schema;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error executing {ToolName}",
                toolName);

            await WriteAuditLogAsync(
                toolName,
                requestData,
                "Spreadsheet schema discovery failed",
                false,
                ex.Message);

            throw;
        }
    }

    [McpServerTool]
    [Description(
        "Searches a specific worksheet and column for a value. " +
        "Use DiscoverSpreadsheet first to identify valid worksheet " +
        "and column names.")]
    public async Task<IReadOnlyList<SpreadsheetSearchResult>>
        SearchSpreadsheet(
            [Description(
                "The exact worksheet name returned by DiscoverSpreadsheet.")]
            string sheetName,

            [Description(
                "The exact column name returned by DiscoverSpreadsheet.")]
            string columnName,

            [Description("The text or value to find.")]
            string searchValue,

            [Description(
                "Search operator: Contains, Equals, StartsWith or EndsWith.")]
            SpreadsheetSearchOperator searchOperator =
                SpreadsheetSearchOperator.Contains,

            [Description(
                "Maximum number of matching rows to return, from 1 to 500.")]
            int maximumResults = 100)
    {
        const string toolName = nameof(SearchSpreadsheet);

        var requestData =
            $"SheetName={sheetName}; " +
            $"ColumnName={columnName}; " +
            $"SearchValue={searchValue}; " +
            $"SearchOperator={searchOperator}; " +
            $"MaximumResults={maximumResults}";

        try
        {
            ValidateSearchText(searchValue);
            ValidateMaximumResults(maximumResults);

            _logger.LogInformation(
                "Tool executed: {ToolName}. " +
                "Sheet={SheetName}, Column={ColumnName}, " +
                "Operator={SearchOperator}, MaximumResults={MaximumResults}",
                toolName,
                sheetName,
                columnName,
                searchOperator,
                maximumResults);

            var results =
                _spreadsheetDatabaseService.Search(
                    sheetName,
                    columnName,
                    searchValue,
                    searchOperator,
                    maximumResults);

            _logger.LogInformation(
                "{ToolName} returned {Count} matching rows",
                toolName,
                results.Count);

            await WriteAuditLogAsync(
                toolName,
                requestData,
                $"{results.Count} matching rows returned",
                true);

            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error executing {ToolName}. " +
                "Sheet={SheetName}, Column={ColumnName}",
                toolName,
                sheetName,
                columnName);

            await WriteAuditLogAsync(
                toolName,
                requestData,
                "Targeted spreadsheet search failed",
                false,
                ex.Message);

            throw;
        }
    }

    [McpServerTool]
    [Description(
        "Searches every used column in every worksheet for a value. " +
        "Use this when the relevant worksheet or column is unknown.")]
    public async Task<IReadOnlyList<SpreadsheetSearchResult>>
        SearchEntireSpreadsheet(
            [Description(
                "The text or value to find anywhere in the spreadsheet.")]
            string searchValue,

            [Description(
                "Maximum number of matching rows to return, from 1 to 500.")]
            int maximumResults = 100)
    {
        const string toolName = nameof(SearchEntireSpreadsheet);

        var requestData =
            $"SearchValue={searchValue}; " +
            $"MaximumResults={maximumResults}";

        try
        {
            ValidateSearchText(searchValue);
            ValidateMaximumResults(maximumResults);

            _logger.LogInformation(
                "Tool executed: {ToolName}. " +
                "SearchValue={SearchValue}, MaximumResults={MaximumResults}",
                toolName,
                searchValue,
                maximumResults);

            var results =
                _spreadsheetDatabaseService.SearchAll(
                    searchValue,
                    maximumResults);

            _logger.LogInformation(
                "{ToolName} returned {Count} matching rows",
                toolName,
                results.Count);

            await WriteAuditLogAsync(
                toolName,
                requestData,
                $"{results.Count} matching rows returned",
                true);

            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error executing {ToolName}",
                toolName);

            await WriteAuditLogAsync(
                toolName,
                requestData,
                "Entire spreadsheet search failed",
                false,
                ex.Message);

            throw;
        }
    }

    /*
     * Contract search and business calculation tools
     */

    [McpServerTool]
    [Description(
        "Searches contract records across supplier, framework title, " +
        "contract manager, service area, classification and reference fields.")]
    public async Task<string> SearchContracts(
        [Description(
            "A supplier, framework title, manager, service area, " +
            "classification or reference value to search for.")]
        string searchText)
    {
        const string toolName = nameof(SearchContracts);
        var requestData = $"SearchText={searchText}";

        try
        {
            ValidateSearchText(searchText);

            _logger.LogInformation(
                "Tool executed: {ToolName}. SearchText={SearchText}",
                toolName,
                searchText);

            var contracts =
                _spreadsheetService.SearchContracts(searchText);

            string response;

            if (contracts.Count == 0)
            {
                response =
                    $"No contracts were found matching '{searchText}'.";
            }
            else
            {
                var contractDetails = contracts.Select(contract =>
                    $"""
                    Reference: {contract.ContractReference}
                    Framework: {contract.FrameworksTitle}
                    Supplier: {contract.Supplier}
                    Service Area: {contract.ServiceArea}
                    Contract Manager: {contract.ContractManager}
                    Total Value: {FormatCurrency(contract.TotalContractValue)}
                    Start Date: {FormatDate(contract.ContractStartDate)}
                    End Date: {FormatDate(contract.ContractEndDate)}
                    """);

                response =
                    $"{contracts.Count} matching contract(s) found." +
                    Environment.NewLine +
                    Environment.NewLine +
                    string.Join(
                        Environment.NewLine + Environment.NewLine,
                        contractDetails);
            }

            _logger.LogInformation(
                "{ToolName} returned {Count} contracts",
                toolName,
                contracts.Count);

            await WriteAuditLogAsync(
                toolName,
                requestData,
                $"{contracts.Count} matching contracts returned",
                true);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error executing {ToolName}. SearchText={SearchText}",
                toolName,
                searchText);

            await WriteAuditLogAsync(
                toolName,
                requestData,
                "Contract search failed",
                false,
                ex.Message);

            return
                "An error occurred while searching the contract records.";
        }
    }

    [McpServerTool]
    [Description(
        "Calculates the total value of contracts that start within " +
        "a specified date range.")]
    public async Task<string> GetTotalContractValueBetweenDates(
        [Description(
            "The start of the date range, using YYYY-MM-DD where possible.")]
        DateTime startDate,

        [Description(
            "The end of the date range, using YYYY-MM-DD where possible.")]
        DateTime endDate)
    {
        const string toolName =
            nameof(GetTotalContractValueBetweenDates);

        var requestData =
            $"StartDate={startDate:yyyy-MM-dd}; " +
            $"EndDate={endDate:yyyy-MM-dd}";

        try
        {
            ValidateDateRange(startDate, endDate);

            _logger.LogInformation(
                "Tool executed: {ToolName}. " +
                "StartDate={StartDate}, EndDate={EndDate}",
                toolName,
                startDate,
                endDate);

            var matchingContracts =
                _spreadsheetService
                    .GetFrameworkContracts()
                    .Where(contract =>
                        contract.ContractStartDate.HasValue &&
                        contract.ContractStartDate.Value.Date >=
                            startDate.Date &&
                        contract.ContractStartDate.Value.Date <=
                            endDate.Date)
                    .ToList();

            var totalValue =
                matchingContracts.Sum(
                    contract =>
                        contract.TotalContractValue ?? 0m);

            var response =
                $"There are {matchingContracts.Count} contract(s) " +
                $"with a start date between " +
                $"{startDate:dd/MM/yyyy} and {endDate:dd/MM/yyyy}." +
                Environment.NewLine +
                $"Their combined total contract value is " +
                $"{totalValue:C2}.";

            _logger.LogInformation(
                "{ToolName} found {Count} contracts with a " +
                "combined value of {TotalValue}",
                toolName,
                matchingContracts.Count,
                totalValue);

            await WriteAuditLogAsync(
                toolName,
                requestData,
                $"{matchingContracts.Count} contracts; " +
                $"total value {totalValue:C2}",
                true);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error executing {ToolName}. " +
                "StartDate={StartDate}, EndDate={EndDate}",
                toolName,
                startDate,
                endDate);

            await WriteAuditLogAsync(
                toolName,
                requestData,
                "Date-range contract value calculation failed",
                false,
                ex.Message);

            return
                "An error occurred while calculating the total " +
                "contract value for the date range.";
        }
    }

    [McpServerTool]
    [Description(
        "Calculates the combined total contract value for a specified " +
        "service area.")]
    public async Task<string> GetServiceSpend(
        [Description(
            "The full or partial service area name to calculate spend for.")]
        string serviceArea)
    {
        const string toolName = nameof(GetServiceSpend);
        var requestData = $"ServiceArea={serviceArea}";

        try
        {
            ValidateSearchText(serviceArea);

            _logger.LogInformation(
                "Tool executed: {ToolName}. ServiceArea={ServiceArea}",
                toolName,
                serviceArea);

            var matchingContracts =
                _spreadsheetService
                    .GetFrameworkContracts()
                    .Where(contract =>
                        !string.IsNullOrWhiteSpace(
                            contract.ServiceArea) &&
                        contract.ServiceArea.Contains(
                            serviceArea,
                            StringComparison.OrdinalIgnoreCase))
                    .ToList();

            var totalSpend =
                matchingContracts.Sum(
                    contract =>
                        contract.TotalContractValue ?? 0m);

            var response =
                matchingContracts.Count == 0
                    ? $"No contracts were found for the service area " +
                      $"'{serviceArea}'."
                    : $"{matchingContracts.Count} contract(s) were found " +
                      $"for service area '{serviceArea}'." +
                      Environment.NewLine +
                      $"Combined total contract value: " +
                      $"{totalSpend:C2}.";

            _logger.LogInformation(
                "{ToolName} found {Count} contracts with total " +
                "service spend of {TotalSpend}",
                toolName,
                matchingContracts.Count,
                totalSpend);

            await WriteAuditLogAsync(
                toolName,
                requestData,
                $"{matchingContracts.Count} contracts; " +
                $"service spend {totalSpend:C2}",
                true);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error executing {ToolName}. " +
                "ServiceArea={ServiceArea}",
                toolName,
                serviceArea);

            await WriteAuditLogAsync(
                toolName,
                requestData,
                "Service spend calculation failed",
                false,
                ex.Message);

            return
                "An error occurred while calculating service spend.";
        }
    }

    [McpServerTool]
    [Description(
        "Counts contracts with an end date within a specified date range " +
        "and returns basic details of the expiring contracts.")]
    public async Task<string> CountContractsExpiringWithinDateRange(
        [Description(
            "The beginning of the expiry date range, using YYYY-MM-DD " +
            "where possible.")]
        DateTime startDate,

        [Description(
            "The end of the expiry date range, using YYYY-MM-DD " +
            "where possible.")]
        DateTime endDate)
    {
        const string toolName =
            nameof(CountContractsExpiringWithinDateRange);

        var requestData =
            $"StartDate={startDate:yyyy-MM-dd}; " +
            $"EndDate={endDate:yyyy-MM-dd}";

        try
        {
            ValidateDateRange(startDate, endDate);

            _logger.LogInformation(
                "Tool executed: {ToolName}. " +
                "StartDate={StartDate}, EndDate={EndDate}",
                toolName,
                startDate,
                endDate);

            var expiringContracts =
                _spreadsheetService
                    .GetFrameworkContracts()
                    .Where(contract =>
                        contract.ContractEndDate.HasValue &&
                        contract.ContractEndDate.Value.Date >=
                            startDate.Date &&
                        contract.ContractEndDate.Value.Date <=
                            endDate.Date)
                    .OrderBy(contract =>
                        contract.ContractEndDate)
                    .ToList();

            string response;

            if (expiringContracts.Count == 0)
            {
                response =
                    $"No contracts are due to expire between " +
                    $"{startDate:dd/MM/yyyy} and {endDate:dd/MM/yyyy}.";
            }
            else
            {
                var contractDetails =
                    expiringContracts.Select(contract =>
                        $"- {contract.FrameworksTitle} | " +
                        $"{contract.Supplier} | " +
                        $"{FormatDate(contract.ContractEndDate)}");

                response =
                    $"{expiringContracts.Count} contract(s) are due to " +
                    $"expire between {startDate:dd/MM/yyyy} and " +
                    $"{endDate:dd/MM/yyyy}." +
                    Environment.NewLine +
                    string.Join(
                        Environment.NewLine,
                        contractDetails);
            }

            _logger.LogInformation(
                "{ToolName} returned {Count} expiring contracts",
                toolName,
                expiringContracts.Count);

            await WriteAuditLogAsync(
                toolName,
                requestData,
                $"{expiringContracts.Count} expiring contracts found",
                true);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error executing {ToolName}. " +
                "StartDate={StartDate}, EndDate={EndDate}",
                toolName,
                startDate,
                endDate);

            await WriteAuditLogAsync(
                toolName,
                requestData,
                "Contract expiry calculation failed",
                false,
                ex.Message);

            return
                "An error occurred while checking contract expiry dates.";
        }
    }

    [McpServerTool]
    [Description(
        "Calculates projected contract spend after applying a percentage " +
        "increase to the current combined contract value.")]
    public async Task<string> CalculateProjectedSpend(
        [Description(
            "The percentage increase to apply. For example, enter 10 " +
            "for a ten percent increase.")]
        decimal percentageIncrease)
    {
        const string toolName = nameof(CalculateProjectedSpend);
        var requestData =
            $"PercentageIncrease={percentageIncrease}";

        try
        {
            if (percentageIncrease < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(percentageIncrease),
                    "The percentage increase cannot be negative.");
            }

            _logger.LogInformation(
                "Tool executed: {ToolName}. " +
                "PercentageIncrease={PercentageIncrease}",
                toolName,
                percentageIncrease);

            var contracts =
                _spreadsheetService.GetFrameworkContracts();

            var currentSpend =
                contracts.Sum(
                    contract =>
                        contract.TotalContractValue ?? 0m);

            var increaseAmount =
                currentSpend * (percentageIncrease / 100m);

            var projectedSpend =
                currentSpend + increaseAmount;

            var response =
                $"Contracts included: {contracts.Count}" +
                Environment.NewLine +
                $"Current combined contract value: " +
                $"{currentSpend:C2}" +
                Environment.NewLine +
                $"Percentage increase: " +
                $"{percentageIncrease:N2}%" +
                Environment.NewLine +
                $"Increase amount: {increaseAmount:C2}" +
                Environment.NewLine +
                $"Projected combined contract value: " +
                $"{projectedSpend:C2}";

            _logger.LogInformation(
                "{ToolName} calculated projected spend of " +
                "{ProjectedSpend} from current spend of {CurrentSpend}",
                toolName,
                projectedSpend,
                currentSpend);

            await WriteAuditLogAsync(
                toolName,
                requestData,
                $"Current spend {currentSpend:C2}; " +
                $"projected spend {projectedSpend:C2}",
                true);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error executing {ToolName}. " +
                "PercentageIncrease={PercentageIncrease}",
                toolName,
                percentageIncrease);

            await WriteAuditLogAsync(
                toolName,
                requestData,
                "Projected spend calculation failed",
                false,
                ex.Message);

            return
                "An error occurred while calculating projected spend.";
        }
    }

    /*
     * Shared validation, formatting and audit helpers
     */

    private static void ValidateSearchText(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
        {
            throw new ArgumentException(
                "A search value must be provided.",
                nameof(searchText));
        }
    }

    private static void ValidateMaximumResults(int maximumResults)
    {
        if (maximumResults < 1 || maximumResults > 500)
        {
            throw new ArgumentOutOfRangeException(
                nameof(maximumResults),
                "Maximum results must be between 1 and 500.");
        }
    }

    private static void ValidateDateRange(
        DateTime startDate,
        DateTime endDate)
    {
        if (endDate.Date < startDate.Date)
        {
            throw new ArgumentException(
                "The end date cannot be earlier than the start date.");
        }
    }

    private static string FormatDate(DateTime? date)
    {
        return date.HasValue
            ? date.Value.ToString("dd/MM/yyyy")
            : "Not recorded";
    }

    private static string FormatCurrency(decimal? value)
    {
        return value.HasValue
            ? value.Value.ToString("C2")
            : "Not recorded";
    }

    private async Task WriteAuditLogAsync(
        string toolName,
        string requestData,
        string responseSummary,
        bool success,
        string? errorMessage = null)
    {
        try
        {
            await _auditService.LogAsync(new AuditLog
            {
                TimestampUtc = DateTime.UtcNow,
                ToolName = toolName,
                RequestData = requestData,
                ResponseSummary = responseSummary,
                Success = success,
                ErrorMessage = errorMessage
            });
        }
        catch (Exception auditException)
        {
            _logger.LogError(
                auditException,
                "Unable to write audit record for tool {ToolName}",
                toolName);
        }
    }
}