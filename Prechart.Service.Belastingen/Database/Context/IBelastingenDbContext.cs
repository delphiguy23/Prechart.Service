using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Prechart.Service.Core.Database;
using Prechart.Service.Core.Service;
using Prechart.Service.Belastingen.Database.Models;
using Prechart.Service.Belastingen.Database.Models.Berekeningen;

namespace Prechart.Service.Belastingen.Database.Context;

public interface IBelastingenDbContext : IDbContext, IInitializable
{
    DbSet<White> White { get; set; }
    DbSet<Green> Green { get; set; }
    DbSet<Woonlandbeginsel> Woonlandbeginsel { get; set; }
    DbSet<Landen> Landen { get; set; }
    DbSet<AOW> AOW { get; set; }
    DbSet<PremiePercentagesSocialeVerzekeringen> PremiePercentagesSocialeVerzekeringen { get; set; }
    DbSet<Berekeningen> Berekeningen { get; set; }

    DatabaseFacade Database { get; }
}