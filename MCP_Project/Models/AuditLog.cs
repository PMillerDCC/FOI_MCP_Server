namespace MCP_Project.Models
{
    public class AuditLog
    {
        public int AuditLogId { get; set; }

        public DateTime TimestampUtc { get; set; }

        public string? UserIdentifier { get; set; }

        public string ToolName { get; set; } = string.Empty;

        public string? RequestData { get; set; }

        public string? ResponseSummary { get; set; }

        public bool Success { get; set; }

        public string? ErrorMessage { get; set; }

        public int? DurationMs { get; set; }

        public string? SourceIpAddress { get; set; }
    }
}
