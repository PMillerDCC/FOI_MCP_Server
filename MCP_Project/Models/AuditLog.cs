namespace MCP_Project.Models
{
    /// <summary>
    /// Represents one auditable MCP tool invocation event.
    /// </summary>
    public class AuditLog
    {
        /// <summary>
        /// Surrogate primary key.
        /// </summary>
        public int AuditLogId { get; set; }

        /// <summary>
        /// UTC timestamp when the event was recorded.
        /// </summary>
        public DateTime TimestampUtc { get; set; }

        /// <summary>
        /// Optional user or caller identifier.
        /// </summary>
        public string? UserIdentifier { get; set; }

        /// <summary>
        /// Logical tool name that was executed.
        /// </summary>
        public string ToolName { get; set; } = string.Empty;

        /// <summary>
        /// Serialized request details captured for troubleshooting.
        /// </summary>
        public string? RequestData { get; set; }

        /// <summary>
        /// Short response summary for reporting.
        /// </summary>
        public string? ResponseSummary { get; set; }

        /// <summary>
        /// Indicates whether execution completed successfully.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Error message when <see cref="Success"/> is false.
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Optional execution duration in milliseconds.
        /// </summary>
        public int? DurationMs { get; set; }

        /// <summary>
        /// Optional caller IP address captured at request time.
        /// </summary>
        public string? SourceIpAddress { get; set; }
    }
}
