namespace MCP_Project.Models
{
    /// <summary>
    /// Describes a queryable data source exposed through MCP tools.
    /// </summary>
    public class DataSourceInfo
    {
        /// <summary>
        /// Display name of the data source.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Technology type, such as SQL database or spreadsheet.
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Business-oriented description of the source content.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// High-level operations supported by this source.
        /// </summary>
        public List<string> Capabilities { get; set; } = new();
    }
}
