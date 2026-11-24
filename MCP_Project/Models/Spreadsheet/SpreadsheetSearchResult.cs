namespace MCP_Project.Models.Spreadsheet
{
    /// <summary>
    /// Represents one matched worksheet row from a spreadsheet search.
    /// </summary>
    public sealed class SpreadsheetSearchResult
    {
        /// <summary>
        /// Worksheet that produced the result.
        /// </summary>
        public string SheetName { get; init; } = string.Empty;

        /// <summary>
        /// 1-based row number in the worksheet.
        /// </summary>
        public int RowNumber { get; init; }

        /// <summary>
        /// Header/value map from the matched row.
        /// </summary>
        public Dictionary<string, string> Values { get; init; } =
            new(StringComparer.OrdinalIgnoreCase);
    }
}
