namespace MCP_Project.Models.Spreadsheet
{
    public sealed class SpreadsheetColumnInfo
    {
        public int ColumnNumber { get; init; }
        public string ColumnLetter { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
    }
}
