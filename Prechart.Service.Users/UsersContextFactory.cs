using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Prechart.Service.Users.Database.Context;

namespace Prechart.Service.Users;

public class PrechartServiceCoreAuditContextFactory : IDesignTimeDbContextFactory<UsersDbContext>
{
    public UsersDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<UsersDbContext>();
        optionsBuilder.UseSqlServer("Server=10.10.10.10;Database=PrechartOnPi;user id=sa;password=Calender365;");

        return new UsersDbContext(optionsBuilder.Options);
    }
}
