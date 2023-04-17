using Autofac;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Prechart.Service.AuditLog.Consumer;
using Prechart.Service.AuditLog.Consumers;
using Prechart.Service.AuditLog.Database.Context;
using Prechart.Service.AuditLog.Models;
using Prechart.Service.AuditLog.Repositories;
using Prechart.Service.AuditLog.Service;
using Prechart.Service.AuditLog.Utils;
using Prechart.Service.AuditLog.Worker;
using Prechart.Service.Core.Configuration;
using Prechart.Service.Core.Service;

namespace Prechart.Service.AuditLog;

public class AuditLogStartup : ServiceStartup
{
    public override void ConfigureAutoFac(ContainerBuilder builder)
    {
        builder.RegisterType<MongoChangeStreamService>().AsImplementedInterfaces().SingleInstance();
        builder.RegisterType<MongoChangeStreamWorker>().AsImplementedInterfaces().SingleInstance();
        builder.RegisterType<AuditLogRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterType<AuditLogService>().AsImplementedInterfaces().InstancePerLifetimeScope();

        builder.Register(c =>
        {
            var config = c.Resolve<IOptions<GeneralConfiguration>>().Value;
            var opt = new DbContextOptionsBuilder<AuditLogDbContext>();
            opt.UseInMemoryDatabase("AuditLog", a => a.EnableNullChecks(false));
            return new AuditLogDbContext(opt.Options);
        }).AsImplementedInterfaces().InstancePerLifetimeScope();

        builder.Register(
            m =>
            {
                var config = m.Resolve<IOptions<GeneralConfiguration>>().Value;
                var client = new MongoClient(config.MongoDb.ConnectionString);
                return client.GetDatabase(config.MongoDb.Database);
            });

        builder.Register(
            m =>
            {
                var config = m.Resolve<IOptions<GeneralConfiguration>>().Value;
                var client = new MongoClient(config.MongoDb.LogConnectionString);
                var database = client.GetDatabase(config.MongoDb.LogDatabase);
                var collection = database.GetCollection<MongoLogs>("ChangeStream");
                return collection;
            });

        builder.Register(
            m =>
            {
                var config = m.Resolve<IOptions<GeneralConfiguration>>().Value;
                var client = new MongoClient(config.MongoDb.LogConnectionString);
                var database = client.GetDatabase(config.MongoDb.LogDatabase);
                var collection = database.GetCollection<MongoAuditLogs>("AuditLogs");
                return collection;
            });

        builder.Register(
            m =>
            {
                var config = m.Resolve<IOptions<GeneralConfiguration>>().Value;
                var client = new MongoClient(config.MongoDb.LogConnectionString);
                var database = client.GetDatabase(config.MongoDb.LogDatabase);
                var collection = database.GetCollection<ControllerLogs>("ControllerLogs");
                return collection;
            });
    }

    public override void ConfigureMessageBus(IReceiveEndpointConfigurator ep, IBusRegistrationContext provider)
    {
        ep.ConfigureConsumer<EntityUpdatedConsumer>(provider);
        ep.ConfigureConsumer<ControllerLogsEventConsumer>(provider);
        ep.ConfigureConsumer<GetAuditTraceConsumer>(provider);
        ep.ConfigureConsumer<PostLogsToDbConsumer>(provider);
    }
}
