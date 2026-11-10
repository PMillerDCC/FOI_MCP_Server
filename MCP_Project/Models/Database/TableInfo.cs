namespace MCP_Project.Models.Database
{
    public class TableInfo
    {
        public string TableName { get; set; } = "";
        public int RowCount { get; set; }
        public List<ColumnInfo> Columns { get; set; } = new();
    }
}
