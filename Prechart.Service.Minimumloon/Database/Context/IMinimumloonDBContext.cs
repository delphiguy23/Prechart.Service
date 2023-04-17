using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Prechart.Service.Core.Database;
using Prechart.Service.Core.Service;

namespace Prechart.Service.Minimumloon.Database.Context;


public interface IMinimumloonDBContext: IDbContext, IInitializable
{
    DbSet<Models.Minimumloon> Minimumloon { get; set; }
    DatabaseFacade Database { get; }
}
