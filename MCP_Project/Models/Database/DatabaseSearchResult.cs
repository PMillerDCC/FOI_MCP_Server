namespace MCP_Project.Models.Database
{
    public class DatabaseSearchResult
    {
        public string TableName { get; set; } = "";
        public Dictionary<string, object?> Values { get; set; } = new();
    }
}
