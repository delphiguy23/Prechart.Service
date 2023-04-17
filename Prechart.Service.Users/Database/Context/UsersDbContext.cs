using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Prechart.Service.AuditLog.Models.Users;
using Prechart.Service.Core.Persistence;
using Prechart.Service.Users.Database.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Prechart.Service.Users.Database.Context;

public class UsersDbContext : IdentityDbContext, IUsersDbContext
{
    private readonly ISaveDatabaseHelper _saveDatabaseHelper;

    public UsersDbContext()
    {
    }

    public UsersDbContext(DbContextOptions<UsersDbContext> options, ISaveDatabaseHelper saveDatabaseHelper)
        : this(options)
    {
        _saveDatabaseHelper = saveDatabaseHelper;
    }

    public UsersDbContext(DbContextOptions<UsersDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ServiceUsers> ServiceUsers { get; set; }

    public async Task Init()
    {
        await Database.MigrateAsync();
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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        var adminId = "02174cf0–9412–4cfe-afbf-59f706d72cf6";
        var roleId = "341743f0-asd2–42de-afbf-59kmkkmk72cf6";

        builder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Name = "SuperAdmin",
            NormalizedName = "SUPERADMIN",
            Id = roleId,
            ConcurrencyStamp = roleId,
        });

        var appUser = new ServiceUsers
        {
            Id = adminId,
            Email = "admin@prechart.com",
            EmailConfirmed = true,
            FirstName = "Admin",
            LastName = "Admin",
            UserName = "ADMIN",
            NormalizedUserName = "ADMIN",
            ApiToken = "JH+C1Fnv72VIXbmM8aS8+UXJ6ci8Bgtn5R1MeOksvdWz11qmVKNvVQrSsbYivtzBkBikwz6s3ycyY4nyf34i/Q==",
            RefreshToken = "dMQa7YJBXc0rgNQeBeeJnabu+mpChoi4NAkO+1WnhqS+A+fRESDU2svYGdWPTH+1OkpzeHeVBPw8TbJ9p/LKXg==",
            Active = true,
        };

        //set user password
        var ph = new PasswordHasher<ServiceUsers>();
        appUser.PasswordHash = ph.HashPassword(appUser, "ADMIN");

        base.OnModelCreating(builder);
        builder.Entity<ServiceUsers>(entity => { entity.HasData(appUser); });

        //set user role to admin
        builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = roleId,
            UserId = adminId,
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }
}
