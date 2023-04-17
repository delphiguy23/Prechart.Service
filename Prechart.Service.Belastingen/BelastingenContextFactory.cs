using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Prechart.Service.Belastingen.Database.Context;

namespace Prechart.Service.Belastingen;

public class BelastingenContextFactory : IDesignTimeDbContextFactory<BelastingenDbContext>
{
    public Prechart.Service.Belastingen.Database.Context.BelastingenDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BelastingenDbContext>();
        optionsBuilder.UseSqlServer("Data Source=.;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;Database=Prechart;");

        return new BelastingenDbContext(optionsBuilder.Options);
    }
}
