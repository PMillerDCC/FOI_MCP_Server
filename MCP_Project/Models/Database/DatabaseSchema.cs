namespace MCP_Project.Models.Database
{
    public class DatabaseSchema
    {
        public string DatabaseName { get; set; } = "";
        public List<TableInfo> Tables { get; set; } = new();
    }
}
