using Microsoft.EntityFrameworkCore;
using Prechart.Service.Core.Persistence;

namespace Prechart.Service.Werkgever.Database.Context;

public class WerkgeverDbContext : DbContext, IWerkgeverDbContext
{
    private readonly ISaveDatabaseHelper _saveDatabaseHelper;

    public WerkgeverDbContext()
    {
    }

    public WerkgeverDbContext(DbContextOptions<WerkgeverDbContext> options, ISaveDatabaseHelper saveDatabaseHelper)
        : this(options) => _saveDatabaseHelper = saveDatabaseHelper;

    public WerkgeverDbContext(DbContextOptions<WerkgeverDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Models.Werkgever> Werkgever { get; set; }
    public virtual DbSet<Models.WerkgeverWhkPremies> WerkgeverWhkPremies { get; set; }
    
    public async Task Init() => await Database.MigrateAsync();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Models.Werkgever>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.LoonheffingenExtentie).HasColumnType("nvarchar(3)");
            entity.Property(e => e.OmzetbelastingExtentie).HasColumnType("nvarchar(3)");

            entity.HasIndex(p => new
            {
                p.Id,
                p.Naam,
            });
        });
        
        modelBuilder.Entity<Models.WerkgeverWhkPremies>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.FlexWerkgever).HasColumnType("decimal(18,4)");
            entity.Property(e => e.FlexWerknemer).HasColumnType("decimal(18,4)");
            entity.Property(e => e.WgaVastWerkgever).HasColumnType("decimal(18,4)");
            entity.Property(e => e.WgaVastWerknemer).HasColumnType("decimal(18,4)");
            entity.Property(e => e.ZwFlex).HasColumnType("decimal(18,4)");
            entity.Property(e => e.Totaal)
                .HasColumnType("decimal(18,4)")
            .HasComputedColumnSql("[FlexWerkgever] + [FlexWerknemer] + [WgaVastWerkgever] + [WgaVastWerknemer] + [ZwFlex]");

            entity.HasIndex(p => new
            {
                p.Id,
                p.WerkgeverId,
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