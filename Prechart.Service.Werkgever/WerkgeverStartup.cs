using Autofac;
using GreenPipes;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Prechart.Service.Core.Configuration;
using Prechart.Service.Core.Persistence;
using Prechart.Service.Core.Service;
using Prechart.Service.Globals.Models.Werkgever;
using Prechart.Service.Werkgever.Consumers;
using Prechart.Service.Werkgever.Database.Context;
using Prechart.Service.Werkgever.Repository;
using Prechart.Service.Werkgever.Service;

namespace Prechart.Service.Werkgever;

public class WerkgeverStartup : ServiceStartup
{
    public override void ConfigureAutoFac(ContainerBuilder builder)
    {
        builder.RegisterType<WerkgeverRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterType<WerkgeverService>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterGenericRequestClient();

        builder.Register(c =>
        {
            var config = c.Resolve<IOptions<GeneralConfiguration>>().Value;
            var opt = new DbContextOptionsBuilder<WerkgeverDbContext>();
            opt.UseSqlServer(config.ConnectionString);
            return new WerkgeverDbContext(opt.Options, c.Resolve<ISaveDatabaseHelper>());
        }).AsImplementedInterfaces().InstancePerLifetimeScope();

        builder.Register(
            m =>
            {
                var config = m.Resolve<IOptions<GeneralConfiguration>>().Value;
                var client = new MongoClient(config.MongoDb.ConnectionString);
                var database = client.GetDatabase(config.MongoDb.Database);
                var collection = database.GetCollection<MongoWerkgeverModel>("Werkgever");
                return collection;
            });
    }

    public override void ConfigureMessageBus(IReceiveEndpointConfigurator ep, IBusRegistrationContext provider)
    {
        ep.UseMessageRetry(r => r.Incremental(
            10,
            TimeSpan.FromSeconds(2),
            TimeSpan.FromSeconds(5)));

        ep.ConfigureConsumer<GetWerkgeversConsumer>(provider);
        ep.ConfigureConsumer<UpsertWerkgeversConsumer>(provider);
        ep.ConfigureConsumer<UpdateKlantWerkgeverConsumer>(provider);
    }
}
