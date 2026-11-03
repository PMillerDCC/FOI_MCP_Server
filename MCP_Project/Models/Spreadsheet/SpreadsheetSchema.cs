namespace MCP_Project.Models.Spreadsheet
{
    public sealed class SpreadsheetSchema
    {
        public string WorkbookName { get; init; } = string.Empty;
        public List<SpreadsheetInfo> Sheets { get; init; } = [];
    }
}
