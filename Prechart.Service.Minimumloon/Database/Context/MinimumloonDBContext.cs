using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Prechart.Service.Core.Database;
using Prechart.Service.Core.Persistence;

namespace Prechart.Service.Minimumloon.Database.Context;

public class MinimumloonDBContext : DbContext, IMinimumloonDBContext
{
    private readonly ISaveDatabaseHelper _saveDatabaseHelper;
    public MinimumloonDBContext()
    {
    }

    public MinimumloonDBContext(DbContextOptions<MinimumloonDBContext> options, ISaveDatabaseHelper saveDatabaseHelper)
        : this(options) => _saveDatabaseHelper = saveDatabaseHelper;

    public MinimumloonDBContext(DbContextOptions<MinimumloonDBContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Models.Minimumloon> Minimumloon { get; set; }

    public async Task Init() => await Database.MigrateAsync();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       
        modelBuilder.Entity<Models.Minimumloon>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Jaar).HasColumnType("int");
            entity.Property(e => e.MinimumloonLeeftijd).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MinimumloonPerMaand).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MinimumloonPerWeek).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MinimumloonPerDag).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MinimumloonPerUur36).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MinimumloonPerUur38).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MinimumloonPerUur40).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MinimumloonRecordActiefVanaf).HasColumnType("DateTime");
            entity.Property(e => e.MinimumloonRecordActiefTot).HasColumnType("DateTime");
            entity.HasIndex(p => new
            {
                p.Id,
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
            return await _saveDatabaseHelper.SaveChangesAsync(this, new[] { "auditlog" }, cancellationToken);
        }

        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }

}
