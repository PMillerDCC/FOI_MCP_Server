using Microsoft.EntityFrameworkCore;
using MCP_Project.Models;

namespace MCP_Project.Data;

/// <summary>
/// Entity Framework Core database context for application persistence.
/// </summary>
/// <remarks>
/// The context currently stores audit events for MCP tool execution so activity can be
/// reviewed for operational support and governance.
/// </remarks>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Initializes a new <see cref="ApplicationDbContext"/>.
    /// </summary>
    /// <param name="options">Database provider and connection configuration.</param>
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Audit trail records written by MCP tools and middleware.
    /// </summary>
    public DbSet<AuditLog> AuditLogs { get; set; }

    /// <summary>
    /// Applies database model constraints.
    /// </summary>
    /// <param name="modelBuilder">EF model builder.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AuditLog>()
            .HasKey(a => a.AuditLogId);

        // Length limits are used to prevent unbounded string growth in audit tables.
        modelBuilder.Entity<AuditLog>()
            .Property(a => a.ToolName)
            .HasMaxLength(100);

        modelBuilder.Entity<AuditLog>()
            .Property(a => a.UserIdentifier)
            .HasMaxLength(100);

        modelBuilder.Entity<AuditLog>()
            .Property(a => a.SourceIpAddress)
            .HasMaxLength(50);
    }
}
