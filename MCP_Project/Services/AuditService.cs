using MCP_Project.Data;
using MCP_Project.Models;

namespace MCP_Project.Services
{
    /// <summary>
    /// Writes MCP execution audit records to the SQL database.
    /// </summary>
    public class AuditService : IAuditService
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new <see cref="AuditService"/>.
        /// </summary>
        /// <param name="context">Database context used to persist audit records.</param>
        public AuditService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Saves a tool audit record for later reporting and troubleshooting.
        /// </summary>
        /// <param name="auditLog">Audit data to persist.</param>
        public async Task LogAsync(AuditLog auditLog)
        {
            _context.AuditLogs.Add(auditLog);
            await _context.SaveChangesAsync();
        }
    }
}
