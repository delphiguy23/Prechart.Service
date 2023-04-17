using Microsoft.EntityFrameworkCore;

namespace Prechart.Service.AuditLog.Database.Context;

public class AuditLogDbContext : DbContext, IAuditLogDbContext
{
    public AuditLogDbContext()
    {
    }

    public AuditLogDbContext(DbContextOptions<AuditLogDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Models.AuditLog> AuditLog { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }
}
