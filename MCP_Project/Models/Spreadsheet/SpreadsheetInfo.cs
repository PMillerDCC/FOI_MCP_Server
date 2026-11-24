namespace MCP_Project.Models.Spreadsheet
{
    /// <summary>
    /// Schema and row-count details for one worksheet.
    /// </summary>
    public sealed class SpreadsheetInfo
    {
        /// <summary>
        /// Worksheet name.
        /// </summary>
        public string SheetName { get; init; } = string.Empty;

        /// <summary>
        /// Header row number used by the service (fixed to row 1 in this implementation).
        /// </summary>
        public int HeaderRowNumber { get; init; }

        /// <summary>
        /// Number of data rows under the header.
        /// </summary>
        public int DataRowCount { get; init; }

        /// <summary>
        /// Column metadata parsed from the header row.
        /// </summary>
        public List<SpreadsheetColumnInfo> Columns { get; init; } = [];
    }
}
