using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Prechart.Service.Core.Persistence;
using Prechart.Service.TimeSheet.Database.Context;

namespace Prechart.Service.Employee.Database.Context;

public class TimeSheetContext : DbContext, ITimeSheetContext
{
    private readonly ISaveDatabaseHelper _saveDatabaseHelper;

    public TimeSheetContext()
    {
    }

    public TimeSheetContext(DbContextOptions<TimeSheetContext> options, ISaveDatabaseHelper saveDatabaseHelper)
        : this(options) => _saveDatabaseHelper = saveDatabaseHelper;

    public TimeSheetContext(DbContextOptions<TimeSheetContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TimeSheet.Database.Models.TimeSheet> TimeSheet { get; set; }

    public async Task Init() => await Database.MigrateAsync();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TimeSheet.Database.Models.TimeSheet>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasIndex(p => new
            {
                p.Id,
                p.PersonId,
                p.Date,
            });
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ////
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (_saveDatabaseHelper != null)
        {
            return await _saveDatabaseHelper.SaveChangesAsync(this, new[] {"auditlog"}, cancellationToken);
        }

        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }
}