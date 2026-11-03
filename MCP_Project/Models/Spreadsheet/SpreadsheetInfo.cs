namespace MCP_Project.Models.Spreadsheet
{
    public sealed class SpreadsheetInfo
    {
        public string SheetName { get; init; } = string.Empty;
        public int HeaderRowNumber { get; init; }

        public int DataRowCount { get; init; }
        public List<SpreadsheetColumnInfo> Columns { get; init; } = [];
    }
}
