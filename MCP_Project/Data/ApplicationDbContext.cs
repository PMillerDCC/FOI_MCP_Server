using Microsoft.EntityFrameworkCore;
using MCP_Project.Models;

namespace MCP_Project.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AuditLog>()
            .HasKey(a => a.AuditLogId);

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
