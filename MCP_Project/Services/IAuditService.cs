using MCP_Project.Models;

namespace MCP_Project.Services
{
    public interface IAuditService
    {
        Task LogAsync(AuditLog auditLog);
    }
}
