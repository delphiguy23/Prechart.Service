using Microsoft.EntityFrameworkCore;
using Prechart.Service.Core.Database;
using Prechart.Service.Core.Service;

namespace Prechart.Service.TimeSheet.Database.Context;

public interface ITimeSheetContext : IDbContext, IInitializable
{
    DbSet<TimeSheet.Database.Models.TimeSheet> TimeSheet { get; set; }
}