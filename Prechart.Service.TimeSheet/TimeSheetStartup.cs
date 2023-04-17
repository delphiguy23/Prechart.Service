using Autofac;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Prechart.Service.Core.Configuration;
using Prechart.Service.Core.Persistence;
using Prechart.Service.Core.Service;
using Prechart.Service.Employee.Database.Context;
using Prechart.Service.TimeSheet.Repositories;
using Prechart.Service.TimeSheet.Service;

namespace Prechart.Service.TimeSheet
{
    public class TimeSheetStartup : ServiceStartup
    {
        public override void ConfigureAutoFac(ContainerBuilder builder)
        {
            builder.RegisterType<TimeSheetRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<TimeSheetService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterGenericRequestClient();

            builder.Register(c =>
            {
                var config = c.Resolve<IOptions<GeneralConfiguration>>().Value;
                var opt = new DbContextOptionsBuilder<TimeSheetContext>();
                opt.UseSqlServer(config.ConnectionString);
                return new TimeSheetContext(opt.Options, c.Resolve<ISaveDatabaseHelper>());
            }).AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}