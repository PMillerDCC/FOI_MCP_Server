namespace MCP_Project.Models.Spreadsheet
{
    /// <summary>
    /// Column metadata derived from worksheet header cells.
    /// </summary>
    public sealed class SpreadsheetColumnInfo
    {
        /// <summary>
        /// 1-based column index.
        /// </summary>
        public int ColumnNumber { get; init; }

        /// <summary>
        /// Column letter in Excel notation.
        /// </summary>
        public string ColumnLetter { get; init; } = string.Empty;

        /// <summary>
        /// Header text used as the logical column name.
        /// </summary>
        public string Name { get; init; } = string.Empty;
    }
}
