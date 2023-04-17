using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Prechart.Service.Werkgever.Database.Context;

namespace Prechart.Service.Belastingen;

public class WerkgeverDbContextFactory : IDesignTimeDbContextFactory<WerkgeverDbContext>
{
    public Prechart.Service.Werkgever.Database.Context.WerkgeverDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WerkgeverDbContext>();
        optionsBuilder.UseSqlServer("Data Source=tiamzon-pc,1433;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;Database=SvcPrechart;");

        return new WerkgeverDbContext(optionsBuilder.Options);
    }
}
