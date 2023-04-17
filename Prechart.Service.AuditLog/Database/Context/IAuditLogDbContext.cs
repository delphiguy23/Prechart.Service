using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Prechart.Service.Core.Database;

namespace Prechart.Service.AuditLog.Database.Context;

public interface IAuditLogDbContext : IDbContext
{
    DbSet<Models.AuditLog> AuditLog { get; set; }

    DatabaseFacade Database { get; }
}
