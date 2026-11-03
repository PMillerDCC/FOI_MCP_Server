namespace MCP_Project.Models.Spreadsheet
{
    public sealed class SpreadsheetSearchResult
    {
        public string SheetName { get; init; } = string.Empty;
        public int RowNumber { get; init; }
        public Dictionary<string, string> Values { get; init; } =
            new(StringComparer.OrdinalIgnoreCase);
    }
}
