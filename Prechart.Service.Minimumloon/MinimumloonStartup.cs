using Autofac;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Prechart.Service.Core.Configuration;
using Prechart.Service.Core.Persistence;
using Prechart.Service.Core.Service;
using Prechart.Service.Minimumloon.Database.Context;
using Prechart.Service.Minimumloon.Repositories;
using Prechart.Service.Minimumloon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prechart.Service.Minimumloon;

public class MinimumloonStartup: ServiceStartup
{
    public override void ConfigureAutoFac(ContainerBuilder builder)
    {
        builder.RegisterType<MinimumloonRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterType<MinimumloonService>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterGenericRequestClient();

        builder.Register(c =>
        {
            var config = c.Resolve<IOptions<GeneralConfiguration>>().Value;
            var opt = new DbContextOptionsBuilder<MinimumloonDBContext>();
            opt.UseSqlServer(config.ConnectionString);
            return new MinimumloonDBContext(opt.Options, c.Resolve<ISaveDatabaseHelper>());
        }).AsImplementedInterfaces().InstancePerLifetimeScope();
    }
}
