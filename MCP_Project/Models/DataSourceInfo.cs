namespace MCP_Project.Models
{
    public class DataSourceInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Capabilities { get; set; } = new();
    }
}
