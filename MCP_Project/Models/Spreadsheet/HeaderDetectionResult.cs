namespace MCP_Project.Models.Spreadsheet
{
    public sealed class HeaderDetectionResult
    {
        public int HeaderRowCount { get; init; }
        public int DataStartRowNumber { get; init; }
        public Dictionary<int, string> Headers { get; init; } = [];
    }
}
