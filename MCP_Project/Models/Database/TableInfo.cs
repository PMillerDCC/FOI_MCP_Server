namespace MCP_Project.Models.Database
{
    /// <summary>
    /// Table schema metadata returned to MCP clients.
    /// </summary>
    public class TableInfo
    {
        /// <summary>
        /// SQL table name.
        /// </summary>
        public string TableName { get; set; } = "";

        /// <summary>
        /// Optional row count when available.
        /// </summary>
        public int RowCount { get; set; }

        /// <summary>
        /// Column definitions for the table.
        /// </summary>
        public List<ColumnInfo> Columns { get; set; } = new();
    }
}
