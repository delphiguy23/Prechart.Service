using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Prechart.Service.Core.Database;
using Prechart.Service.Core.Service;

namespace Prechart.Service.Werkgever.Database.Context;

public interface IWerkgeverDbContext: IDbContext, IInitializable
{
    DbSet<Models.Werkgever> Werkgever { get; set; }   
    DbSet<Models.WerkgeverWhkPremies> WerkgeverWhkPremies { get; set; }   
    
    DatabaseFacade Database { get; }
}