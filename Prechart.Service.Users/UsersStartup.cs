using Autofac;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Prechart.Service.Core.Configuration;
using Prechart.Service.Core.Persistence;
using Prechart.Service.Core.Service;
using Prechart.Service.Users.Database.Context;
using Prechart.Service.Users.Database.Models;
using Prechart.Service.Users.Helper;
using Prechart.Service.Users.Services;

namespace Prechart.Service.Users;

public class UsersStartup : ServiceStartup
{
    public override void ConfigureAutoFac(ContainerBuilder builder)
    {
        builder.RegisterType<UsersService>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterType<UserTokenHelper>().AsImplementedInterfaces().InstancePerLifetimeScope();

        builder.Register(c =>
        {
            var config = c.Resolve<IOptions<GeneralConfiguration>>().Value;
            var opt = new DbContextOptionsBuilder<UsersDbContext>();
            opt.UseSqlServer(config.ConnectionString);
            return new UsersDbContext(opt.Options, c.Resolve<ISaveDatabaseHelper>());
        }).AsImplementedInterfaces().InstancePerLifetimeScope();

        builder.RegisterType<UsersDbContext>().AsSelf().InstancePerLifetimeScope();
        builder.RegisterGenericRequestClient();
    }

    public override void RegisterConfiguration(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UsersDbContext>(options =>
        {
            var general = configuration.GetSection("General");
            var connectionString = general["ConnectionString"];
            options.UseSqlServer(connectionString);
        });

        services.AddDefaultIdentity<ServiceUsers>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<UsersDbContext>();
    }
}
