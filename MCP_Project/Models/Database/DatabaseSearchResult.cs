namespace MCP_Project.Models.Database
{
    /// <summary>
    /// Represents one row returned from a SQL search tool.
    /// </summary>
    public class DatabaseSearchResult
    {
        /// <summary>
        /// Source table for this result row.
        /// </summary>
        public string TableName { get; set; } = "";

        /// <summary>
        /// Column/value map for the row.
        /// </summary>
        public Dictionary<string, object?> Values { get; set; } = new();
    }
}
