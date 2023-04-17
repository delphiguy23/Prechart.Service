using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Prechart.Service.Employee.Database.Context;

namespace Prechart.Service.TimeSheet;

public class PrechartServiceCoreAuditContextFactory : IDesignTimeDbContextFactory<TimeSheetContext>
{
    public TimeSheetContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TimeSheetContext>();
        optionsBuilder.UseSqlServer("Server=10.10.10.30;Database=PrechartAuditLog;user id=sa;password=Calender365;");

        return new TimeSheetContext(optionsBuilder.Options);
    }
}