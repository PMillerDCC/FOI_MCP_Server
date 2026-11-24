using MCP_Project.Models;

namespace MCP_Project.Services
{
    /// <summary>
    /// Defines audit persistence operations for tool execution telemetry.
    /// </summary>
    public interface IAuditService
    {
        /// <summary>
        /// Persists one audit event.
        /// </summary>
        /// <param name="auditLog">Audit payload describing one tool invocation.</param>
        Task LogAsync(AuditLog auditLog);
    }
}
