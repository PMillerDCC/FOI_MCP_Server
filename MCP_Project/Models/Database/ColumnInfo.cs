namespace MCP_Project.Models.Database
{
    /// <summary>
    /// Describes one SQL column returned by schema discovery.
    /// </summary>
    public class ColumnInfo
    {
        /// <summary>
        /// SQL column name.
        /// </summary>
        public string ColumnName { get; set; } = "";

        /// <summary>
        /// SQL data type name.
        /// </summary>
        public string DataType { get; set; } = "";

        /// <summary>
        /// Indicates whether the column accepts NULL values.
        /// </summary>
        public bool IsNullable { get; set; }
    }
}
