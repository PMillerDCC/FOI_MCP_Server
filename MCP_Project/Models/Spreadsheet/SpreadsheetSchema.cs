namespace MCP_Project.Models.Spreadsheet
{
    /// <summary>
    /// Workbook-level schema information discovered from the spreadsheet source.
    /// </summary>
    public sealed class SpreadsheetSchema
    {
        /// <summary>
        /// File name of the discovered workbook.
        /// </summary>
        public string WorkbookName { get; init; } = string.Empty;

        /// <summary>
        /// Per-sheet schema details.
        /// </summary>
        public List<SpreadsheetInfo> Sheets { get; init; } = [];
    }
}
