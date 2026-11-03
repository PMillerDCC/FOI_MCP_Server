using System.ComponentModel;
using MCP_Project.Models.Spreadsheet;
using MCP_Project.Services;
using ModelContextProtocol.Server;

namespace MCP_Project.Tools;

[McpServerToolType]
public sealed class SpreadsheetDatabaseTools
{
    private readonly SpreadsheetDatabaseService _service;

    public SpreadsheetDatabaseTools(
        SpreadsheetDatabaseService service)
    {
        _service = service;
    }

    [McpServerTool]
    [Description(
        "Discovers the spreadsheet structure, including " +
        "worksheet names, column names and data-row counts.")]
    public SpreadsheetSchema DiscoverSpreadsheet()
    {
        return _service.DiscoverSchema();
    }

    [McpServerTool]
    [Description(
    "Searches a named spreadsheet worksheet and column. " +
    "Call DiscoverSpreadsheet first to identify valid " +
    "worksheet and column names.")]
    public IReadOnlyList<SpreadsheetSearchResult>
    SearchSpreadsheet(
        [Description(
            "The exact worksheet name returned by " +
            "DiscoverSpreadsheet.")]
        string sheetName,

        [Description(
            "The exact column name returned by " +
            "DiscoverSpreadsheet.")]
        string columnName,

        [Description("The value to find.")]
        string searchValue,

        [Description(
            "Search operator: Contains, Equals, " +
            "StartsWith or EndsWith.")]
        SpreadsheetSearchOperator searchOperator =
            SpreadsheetSearchOperator.Contains,

        [Description(
            "Maximum number of rows to return, from 1 to 500.")]
        int maximumResults = 100)
    {
        return _service.Search(
            sheetName,
            columnName,
            searchValue,
            searchOperator,
            maximumResults);
    }

    [McpServerTool]
    [Description(
    "Searches all used columns in all worksheets for a value. " +
    "Use this when the relevant worksheet or column is unknown.")]
    public IReadOnlyList<SpreadsheetSearchResult>
    SearchEntireSpreadsheet(
        [Description("The text or value to find.")]
        string searchValue,

        [Description(
            "Maximum number of matching rows to return, " +
            "from 1 to 500.")]
        int maximumResults = 100)
    {
        return _service.SearchAll(
            searchValue,
            maximumResults);
    }
}
